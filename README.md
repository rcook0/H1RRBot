# H1RRBot — H1 Risk/Reward Trading Bot

This repository contains a trading bot and supporting scripts implementing a 1-hour timeframe risk/reward strategy for Gold (XAUUSD), Bitcoin (BTCUSD), and GBPJPY.

---

## Features

- **Timeframe:** H1 (1 Hour) for stable signals  
- **Strategy:** Bullish/Bearish candle patterns confirmed with EMA trend filters  
- **Risk/Reward:** Default 2:1, configurable  
- **Symbols:** XAUUSD, BTCUSD, GBPJPY  
- **Session Filters:** London and New York trading sessions  
- **Platforms:**  
  - cTrader (.algo and .cs files)  
  - TradingView (Pine Script strategy and study)  
- **Alerts:** Telegram & Discord webhook templates included  
- **Position Sizing:** Auto lot sizing with manual override

---

## Getting Started

### cTrader

1. Open cTrader Automate.  
2. Import `H1RRTraderBot.algo` or `H1RRTraderBot.cs`.  
3. Attach to an H1 chart.  
4. Configure parameters (symbol, SL, RR, sessions).  
5. Start the bot.

### TradingView

1. Open Pine Script editor.  
2. Paste `H1_RR_Strategy.pinescript` or `H1_RR_Study.pinescript`.  
3. Add to chart.  
4. Create alerts using webhook templates if desired.

---

## Alerts Setup

- Use provided Telegram and Discord webhook templates in `Docs/AlertTemplates.txt`  
- Replace placeholder tokens with your actual bot tokens and IDs

---

## Repository Structure

