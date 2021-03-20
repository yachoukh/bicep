// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

import * as spawn from 'cross-spawn';
import * as path from "path";
import * as fs from "fs";
import { SpawnSyncReturns } from 'node:child_process';

export interface Example {
  projectDir: string,
  projectFile: string
}

export function createExample(projectName: string): Example {
  const projectDir = path.normalize(path.join(__dirname, `../examples/${projectName}/`))
  return {
    projectDir: projectDir,
    projectFile: path.join(projectDir, `${projectName}.proj`)
  }
};

export function cleanExample(example: Example) {
  const result = spawn.sync("git", ["clean", "-dfx"], {
    cwd: example.projectDir,
    stdio: "pipe",
    encoding: "utf-8",
  });

  if(result.status != 0) {
    handleFailure("git", result);
  }
}

export function buildExample(example: Example, runtimeSuffix: string, expectSuccess: boolean = true) {
  const result = spawn.sync("dotnet", ["build", `/p:RuntimeSuffix=${runtimeSuffix}`, "/bl", example.projectFile], {
    cwd: example.projectDir,
    stdio: "pipe",
    encoding: "utf-8",
  });

  if(expectSuccess && result.status != 0) {
    handleFailure("MSBuild", result);
  }

  return result;
}

export function expectTemplate(example: Example, relativeFilePath: string) {
  const filePath = path.join(example.projectDir, relativeFilePath);
  expect(fs.existsSync(filePath)).toBeTruthy();

  try {
    JSON.parse(fs.readFileSync(filePath, { encoding: "utf-8" }));
  } catch (error) {
    fail(error);
  }
}

function handleFailure(tool: string, result: SpawnSyncReturns<string>) {
  if (result.stderr.length > 0) {
    fail(result.stderr);
  }

  if (result.stdout.length > 0) {
    fail(result.stdout);
  }

  fail(`${tool} exit code ${result.status}`);
}
