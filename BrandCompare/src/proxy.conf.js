const PROXY_CONFIG = [
  {
    context: [
      "/weatherforecast",
      "/brands"
    ],
    target: "https://localhost:7292",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
