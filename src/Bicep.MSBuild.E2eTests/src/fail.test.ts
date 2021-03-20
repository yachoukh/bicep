// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

import * as shared from "./shared";

test("fail", ()=>{
  const example = shared.createExample("fail");
  shared.cleanExample(example);

  const result = shared.buildExample(example, "win-x64", false);

  expect(result.status).not.toBe(0);
})
