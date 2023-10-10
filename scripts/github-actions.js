// @ts-check

// GitHub token
export const GITHUB_TOKEN = process.env.GITHUB_TOKEN ?? '';

// git tag
const prefix = 'refs/tags/';
const ref = process.env.GITHUB_REF ?? '';
export const GIT_TAG = ref.startsWith(prefix) ? ref.substring(prefix.length) : '';

// Repo slug, e.g. "owner_name/repo_name"
export const REPO = process.env.GITHUB_REPOSITORY ?? '';
