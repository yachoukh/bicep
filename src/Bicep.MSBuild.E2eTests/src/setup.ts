// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

import * as spawn from "cross-spawn";
import * as path from "path";
beforeAll(()=>{
  const result = spawn.sync("git", ["clean", "-dfx"], {
    cwd: path.join(__dirname, "../examples")
  });
  expect(result.status).toBe(0);
});
