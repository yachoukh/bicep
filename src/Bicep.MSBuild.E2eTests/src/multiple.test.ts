// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

import * as shared from "./shared";

test("multiple", ()=>{
  const example = shared.createExample("multiple");
  shared.cleanExample(example);

  const result = shared.buildExample(example, "win-x64");

  expect(result.stderr).toBe('');

  shared.expectTemplate(example, "bin/debug/templates/empty.json");
  shared.expectTemplate(example, "bin/debug/templates/passthrough.json");
  shared.expectTemplate(example, "bin/debug/templates/special/special.arm");
})
