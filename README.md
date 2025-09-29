# Injex

Injex is a lightweight dependency injection container designed for developers who want to
understand how DI works under the hood. It’s minimal,
fast, and perfect as both a learning resource and a foundation for small projects.

---

## 📂 Project Structure
- `src/` – main library code
- `tests/` – unit tests (xUnit)
- `samples/` – sample usage (console app)
- `.github/workflows/ci.yml` – GitHub Actions workflow (build + test)
- `Directory.Build.props` – shared build settings (nullable enabled, warnings as errors, etc.)

---

## 🗺️ Roadmap

- [ ] **Service Registration**
    - Be able to register services into a service collection (by type, factory, or instance).
- [X] **Container Creation**
    - Build a container/service provider based on the service collection.
- [X] **Simple Resolution**
    - Retrieve a service without dependencies from the container.
- [X] **Constructor Injection**
    - Retrieve a service with dependencies automatically provided by the container.
- [X] **Cycle detection**
  - Detect circular dependencies during resolution and throw a clear exception with the full dependency path.
---
## 🚀 Getting Started
