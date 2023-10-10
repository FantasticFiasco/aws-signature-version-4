// @ts-check

import { readdir } from 'fs/promises';
import { join } from 'path';
import { GITHUB_TOKEN, GIT_TAG, REPO } from './github-actions.js';
import { createRelease, uploadAsset } from './github.js';
import { fatal } from './log.js';

/**
 * A tagged commit in this repo is created using the following format:
 *
 *   v<version>
 *
 * where <version> is the semantic version (SemVer) of the package.
 *
 * The following tag would satisfy the format:
 *
 *   v1.2.3-alpha
 */
const assertGitTag = () => {
    if (!GIT_TAG) {
        fatal('Aborting a deployment to GitHub Releases because this is not a tagged commit');
        return null;
    }

    // Remove the 'v' prefix and assert that it's a valid SemVer version
    const tag = GIT_TAG.replace(/^v/, '');
    const semverRegex = /^(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)(?:-((?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+([0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$/gm;
    if (!semverRegex.test(tag)) {
        fatal(`Aborting deployment to GitHub Releases because "${tag}" does not conform to SemVer`);
    }

    return GIT_TAG;
};

const parseRepo = () => {
    const [owner, repo] = REPO.split('/');

    return {
        owner,
        repo,
    };
};

const main = async () => {
    const version = assertGitTag();
    if (!version) {
        return;
    }

    const { owner, repo } = parseRepo();

    // Create GitHub release
    const { releaseId } = await createRelease(GITHUB_TOKEN, owner, repo, GIT_TAG, version);

    const distDir = join('..', 'dist');
    for (const asset of await readdir(distDir)) {
        const fileName = join(distDir, asset);
        await uploadAsset(GITHUB_TOKEN, owner, repo, releaseId, fileName);
    }
};

main().catch((err) => {
    fatal(err);
});
