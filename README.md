# **Systematic Hedging Strategy Backtesting Tool**

## **Project Overview**

This project aims to develop a backtesting application for systematic hedging strategies on Basket Options. Built using the .NET framework and C#, the application is designed as a decision-support tool, enabling both backtesting and forward testing for portfolios that track multiple assets. By employing various rebalancing strategies, the application can simulate and analyze portfolio performance under dynamic market conditions.

The project includes several components:
- **BacktestConsole**: A command-line application for running backtests and generating outputs based on test parameters and market data.
- **gRPC Server and Client**: The gRPC server handles portfolio replication computations, while the client invokes the server to retrieve and process data results.

---

## **Features**

1. **BacktestConsole**
   - **Functionality**: Runs backtests on hedging portfolios with rebalancing strategies applied at each time step.
   - **Deltas**: Calculates the sensitivities of the portfolio to underlying asset price movements, used to update portfolio composition.
   - **Output**: Provides a JSON output of the portfolio’s performance over time.

   **Usage Example:**
   ```bash
   BacktestConsole.exe <test-params> <market-data> <output-file>
>
2. **gRPC Server and Client (BacktestEvaluation)**

  - **Server**: Handles computation requests for portfolio replication based on market and test data.
  - **Client**: Invokes the server and retrieves computed values without performing calculations directly, enabling efficient data handling.
  - **Configuration**: The server listens on https://localhost:7177 for secure communication.
