import type { ZudokuConfig } from "zudoku";

const config: ZudokuConfig = {
  basePath: "/DigitalShop_Sample/zudoku",
  topNavigation: [
    { id: "docs", label: "Documentation" },
    { id: "api", label: "API Reference" },
  ],
  sidebar: {
    docs: [
      {
        type: "category",
        label: "Overview",
        items: ["docs/about", "docs/authorization", "docs/installation"],
      },
    ],
  },
  redirects: [{ from: "/", to: "/docs/about" }],
  apis: {
    type: "file",
    input: "./apis/swagger.json",
    navigationId: "api",
  },
  docs: {
    files: "/pages/**/*.{md,mdx}",
  },
};

export default config;
