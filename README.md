# 🤖 AI Agent Orchestrator (C# / .NET)

A **production-minded, test-driven reference implementation** of an AI agent in C#, designed to demonstrate **safe orchestration of LLM reasoning and deterministic tool execution**.

This project intentionally focuses on **architecture, safety, observability, and testability**, rather than prompt tricks or vendor lock-in.

---

## ✨ Key Design Principles

- **Separation of concerns**
  - LLMs reason
  - Tools execute
  - The agent orchestrates
- **Fail-closed safety**
  - Malformed or ambiguous AI output cannot cause side effects
- **Human-in-the-loop enforcement**
  - Explicit escalation paths are built into the agent
- **Deterministic execution**
  - Tools are pure, testable components
- **Observability first**
  - Telemetry is treated as a first-class behavior
- **Provider agnostic**
  - OpenAI is one implementation, not a dependency

---

## 🚀 Quick Start for Reviewers

If you are reviewing this repository for architectural quality, safety, or AI system design, here is the fastest way to evaluate it:

### 1️⃣ Start with the Agent Engine
**File:** `Agent/AgentEngine.cs`

This is the orchestration layer that:
- Accepts user input
- Delegates reasoning to the LLM decision service
- Enforces safety boundaries
- Executes deterministic tools
- Emits telemetry

> This file answers: *“How does this system prevent unsafe AI behavior?”*

---

### 2️⃣ Review the Decision Contract
**Files:**
- `Agent/AgentDecision.cs`
- `Agent/IAgentDecisionService.cs`

The LLM is constrained to produce **structured, validated decisions**.
Malformed or ambiguous output:
- Cannot execute tools
- Forces human review
- Is fully observable

> This answers: *“How do we trust probabilistic output?”*

---

### 3️⃣ Inspect Tool Isolation
**Folder:** `Tools/`

Each tool:
- Implements `ITool`
- Is deterministic and testable
- Has no hidden side effects
- Cannot be invoked directly by the LLM

> This answers: *“Where does real work happen — and how is it controlled?”*

---

### 4️⃣ Look at the Tests (Most Important)
**Project:** `AiAgent.Orchestrator.Tests`

Recommended starting points:
- `AgentEngineTests`
- Tool-specific unit tests
- Telemetry verification tests

These tests intentionally focus on:
- Malformed LLM output
- Unknown tool names
- Safety enforcement
- Non-happy-path behavior

> This answers: *“What happens when the AI is wrong?”*

---

### 5️⃣ (Optional) Run the Agent
Running a live model is optional and not required to evaluate the system.

```bash
dotnet test
```

## 🧠 High-Level Architecture

```text
User Input
   ↓
AgentEngine
   ↓
IAgentDecisionService (LLM reasoning)
   ↓
AgentDecision
   ↓
IToolRegistry → ITool
   ↓
Deterministic Tool Execution
   ↓
AgentResult + Telemetry
```

### Important Boundaries

- The **LLM never executes tools**
- The **agent never trusts unvalidated output**
- All execution paths are observable and testable

---


## 🧩 Core Components

### Agent
- `AgentEngine` — Orchestration and control flow
- `AgentDecision` — Structured output from LLM reasoning
- `AgentResult` — Final outcome (success, failure, or review)

### Decision Layer
- `IAgentDecisionService`
- `LlmAgentDecisionService` (LLM-backed)

### Tools
- `ITool` — Deterministic, side-effect-controlled operations
- `SummarizeTextTool`
- `ClassifyIntentTool`

### Telemetry
- `IAgentTelemetry`
- `ConsoleAgentTelemetry`
- Fully injectable and testable

### Infrastructure
- Provider-agnostic interfaces:
  - `ILlmClient`
  - `ITextCompletionService`
- OpenAI implementation provided, but optional

---

## 🧪 Testing Strategy

This project deliberately emphasizes **testing AI failure modes**, not just happy paths.

### Agent-Level Tests

- ❌ Malformed LLM output → **blocked**
- ⚠️ Hallucinated tool name → **graceful failure**
- ✅ Valid decision → **tool executes exactly once**
- 🔗 Composite flow → **multi-step orchestration**

### Tool-Level Tests

- Tools tested in isolation with fake completion services
- No reliance on real models in tests

### Telemetry Tests

- Telemetry events are asserted **explicitly and in order**
- Observability is treated as a contract

> The LLM is never unit-tested.  
> **Decision handling is.**

---

## 🔍 Safety Guarantees

This system guarantees that:

- Malformed AI output **cannot execute tools**
- Unknown or hallucinated tools **cannot execute**
- Human review is **explicit and auditable**
- All AI-driven decisions are observable
- No tool executes without deterministic confirmation

---

## 🚀 Running the Agent (Optional)

A real OpenAI client is included but **not required** to understand or evaluate the architecture.

### With a live model

Set your API key via environment variable:

```bash
export OPENAI_API_KEY="your-key-here"
```

Run the agent from the command line:

```bash
dotnet run --project AiAgent.Orchestrator
```

⚠️ **Live model usage incurs cost.**  
The project is fully functional **without running a real model**.

---

## 🧰 Dependency Injection & Hosting

The project uses:

- .NET Generic Host
- Explicit DI registration in `Startup.cs`
- Options pattern for configuration
- No magic scanning or hidden wiring

This makes the system:

- Console-friendly
- Cloud-ready
- API-ready
- Background-service-ready

---

## 🎯 What This Project Is (and Isn’t)

### This is:

- A reference architecture
- A safety-first agent design
- A hiring-grade demonstration of AI system thinking

### This is not:

- A chatbot
- A prompt engineering demo
- A vendor-locked SDK wrapper
- A speculative AGI project

---

## 🧭 Why This Exists

Most AI agent examples focus on *what the model can do*.

This project focuses on:

- What the system must **never** do
- How to contain uncertainty
- How to test probabilistic systems
- How to build AI systems engineers can trust

---

## 📄 License

MIT