// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

import * as shared from "./shared";

test("simple", () => {
  const example = shared.createExample("simple");
  shared.cleanExample(example);

  const result = shared.buildExample(example, "win-x64");

  expect(result.stderr).toBe('');

  shared.expectTemplate(example, "bin/debug/net472/empty.json");
});
