# 🎮 RetroGamesLauncher

**RetroGamesLauncher** é um aplicativo em WPF desenvolvido em C# com o objetivo de oferecer uma interface moderna, simples e responsiva para iniciar jogos retrô através de emuladores como NES, SNES, entre outros. Ele permite ao usuário visualizar, organizar e iniciar seus jogos favoritos com praticidade.

---

## ✨ Funcionalidades

- 🔍 **Pesquisa dinâmica** com debounce
- 📚 **Lista paginada** de jogos (carregamento incremental)
- 🖼️ Exibição de **imagens de capa** e **screenshots** dos jogos
- 🕹️ **Execução automática** do emulador com a ROM selecionada
- 📂 Painel de ações expansível com animação
- ⌨️ **Atalhos globais** para fechar emuladores em execução
- 💾 Armazenamento dos dados dos jogos em **SQLite**
- 🔒 Suporte a futuras configurações e personalizações

---

<img width="899" height="594" alt="image" src="https://github.com/user-attachments/assets/dbe89779-cf42-41e3-8620-51ef0b5cc917" />

---

## 📦 Tecnologias

- [.NET 8](https://dotnet.microsoft.com/)
- [WPF (Windows Presentation Foundation)](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/)
- [SQLite](https://www.sqlite.org/index.html)
- MVVM e injeção de dependência com repositórios

---

## 🚀 Como executar

1. **Clone o repositório:**
   ```bash
   git clone https://github.com/seu-usuario/RetroGamesLauncher.git
---

## 🗃️ Banco de Dados
O launcher utiliza SQLite para armazenar os dados dos jogos, incluindo:

- Nome

- Caminho da ROM

- Caminho da imagem e screenshot

- Emulador associado

---

## 🛠️ Futuras melhorias

 - Suporte a múltiplos emuladores com configurações específicas

 - Interface de adição de jogos

 - Suporte para jogos organizados por plataforma

 - Suporte a joysticks/gamepads

---
