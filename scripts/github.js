// @ts-check

import { Octokit } from '@octokit/rest';
import { readFileSync } from 'fs';
import { basename } from 'path';
import { info } from './log.js';

/**
 * @param {string} githubToken
 * @param {string} owner
 * @param {string} repo
 * @param {string} tagName
 * @param {string} version
 */
export const createRelease = async (githubToken, owner, repo, tagName, version) => {
    info(`github: create release from tag ${tagName}`);

    const octokit = new Octokit({
        auth: githubToken,
    });

    const release = await octokit.repos.createRelease({
        owner,
        repo,
        tag_name: tagName,
        name: `Release ${version}`,
        body: 'TODO',
        draft: true,
    });

    return {
        releaseId: release.data.id,
    };
};

/**
 * @param {string} githubToken
 * @param {string} owner
 * @param {string} repo
 * @param {number} releaseId
 * @param {string} assetFileName
 */
export const uploadAsset = async (githubToken, owner, repo, releaseId, assetFileName) => {
    info(`github: upload asset ${assetFileName}`);

    const octokit = new Octokit({
        auth: githubToken,
    });

    await octokit.repos.uploadReleaseAsset({
        owner,
        repo,
        release_id: releaseId,
        name: basename(assetFileName),
        data: readFileSync(assetFileName).toString(),
    });
};
