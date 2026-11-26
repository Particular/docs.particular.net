#!/usr/bin/env bash
# Checks that a moved page keeps its old URL.
#
# A page that is moved or renamed must list its old URL in the `redirects:`
# front matter of a page. Without the redirect, existing links (release notes,
# announcements, emails, search results) return 404.
#
# Usage: tools/validate-redirects.sh <base-ref> [<head-ref>]
#
# - Moved or renamed page without a redirect: error, exit code 1.
# - Deleted page without a redirect: warning only.

set -euo pipefail

base="${1:?Usage: $0 <base-ref> [<head-ref>]}"
head="${2:-HEAD}"

# Prints the URL of a page, or nothing when the file is not a page.
page_url() {
  local path="$1"
  case "$path" in
    */*) ;;
    *) return 0 ;; # Files in the repository root are not pages.
  esac
  case "$path" in
    *.partial.md | *.include.md) return 0 ;;
    .github/* | menu/* | components/* | Snippets/* | tools/* | tests/*) return 0 ;;
  esac

  local url="${path%.md}"
  local name="${url##*/}"
  case "$path" in
    samples/*)
      # In samples, only sample.md and index.md files are pages.
      case "$name" in
        sample | index) url="${url%/*}" ;;
        *) return 0 ;;
      esac
      ;;
    *)
      if [ "$name" = "index" ]; then
        url="${url%/*}"
      fi
      ;;
  esac
  echo "$url"
}

# Prints all redirect values of all pages at the head ref, one per line.
redirects_at_head() {
  git grep -h -A30 '^redirects:' "$head" -- '*.md' |
    awk '
      /^redirects:/ { inlist = 1; next }
      inlist && /^[[:space:]]*-[[:space:]]*/ {
        value = $0
        sub(/^[[:space:]]*-[[:space:]]*/, "", value)
        gsub(/["'"'"'\r]/, "", value)
        sub(/^\/+/, "", value)
        sub(/\/+$/, "", value)
        sub(/[[:space:]]+$/, "", value)
        print tolower(value)
        next
      }
      { inlist = 0 }
    ' | sort -u
}

known_redirects="$(redirects_at_head)"

has_redirect() {
  grep -qxF "$(echo "$1" | tr '[:upper:]' '[:lower:]')" <<<"$known_redirects"
}

errors=0
warnings=0

while IFS=$'\t' read -r status old new; do
  old_url="$(page_url "$old")"
  [ -n "$old_url" ] || continue

  case "$status" in
    R*)
      new_url="$(page_url "$new")"
      if [ "$old_url" = "$new_url" ]; then
        continue
      fi
      if ! has_redirect "$old_url"; then
        echo "::error file=$new::The page moved from '$old_url'. Add the old URL to the front matter of '$new': redirects: - $old_url"
        errors=$((errors + 1))
      fi
      ;;
    D*)
      if ! has_redirect "$old_url"; then
        echo "::warning file=$old::The page '$old_url' is deleted. If a page replaces it, add '$old_url' to the 'redirects:' front matter of that page."
        warnings=$((warnings + 1))
      fi
      ;;
  esac
done < <(git diff --find-renames --diff-filter=RD --name-status "$base...$head" -- '*.md')

echo "Moved pages without a redirect: $errors. Deleted pages without a redirect: $warnings."

if [ "$errors" -gt 0 ]; then
  echo "See 'Redirects' in README.md. If a redirect is not wanted, add the 'no-redirect-needed' label to the pull request."
  exit 1
fi
