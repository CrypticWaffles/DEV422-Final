
#!/usr/bin/env bash
# baseline-migrations.sh
# Creates & applies an empty baseline migration for each service project.

set -eu
set -o pipefail

# Projects living under the current folder:
PROJECTS=("PlayerManagementService" "TeamManagementService" "PerformanceTrackingService")

new_baseline_name() {
  date +"Baseline_%Y%m%d_%H%M%S"
}

for proj in "${PROJECTS[@]}"; do
  echo -e "\n===> Baseline for project: $proj"
  if [ ! -d "$proj" ]; then
    echo "Project path not found: $proj" >&2
    continue
  fi

  pushd "$proj" >/dev/null

  baseline_name=$(new_baseline_name)
  echo "Adding empty migration: $baseline_name"
  dotnet ef migrations add "$baseline_name"

  migrations_dir="Migrations"
  migration_file=$(ls "$migrations_dir"/*_"$baseline_name".cs 2>/dev/null | head -n1 || true)

  if [ -z "${migration_file}" ]; then
    echo "Could not find migration file for $baseline_name under $migrations_dir" >&2
    popd >/dev/null
    continue
  fi

  echo "Editing file: $migration_file"
  # Replace bodies of Up/Down methods with empty blocks (baseline marker)
  perl -0777 -pe \
    "s/protected override void Up\(MigrationBuilder migrationBuilder\)\s*\{.*?\}/protected override void Up(MigrationBuilder migrationBuilder)\n    {\n        // Baseline: no schema changes\n    }/s" \
    -i "$migration_file"

  perl -0777 -pe \
    "s/protected override void Down\(MigrationBuilder migrationBuilder\)\s*\{.*?\}/protected override void Down(MigrationBuilder migrationBuilder)\n    {\n        // Baseline: no schema changes\n    }/s" \
    -i "$migration_file"

  echo "Applying baseline migration to database…"
  dotnet ef database update

  popd >/dev/null
  echo "Baseline applied for $proj"
done

echo -e "\nAll baselines processed. Next: add real migrations for model changes (e.g., dotnet ef migrations add AddNewColumnToX)"
