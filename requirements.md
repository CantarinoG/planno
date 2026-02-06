# 1. Objetivo Geral

Desenvolver um aplicativo web de agenda, funcionalmente semelhante ao **Google Calendar**, executável imediatamente após a extração de um arquivo ZIP, utilizando **Docker Compose**, rodando em `localhost`.

# 2. Execução e Entrega

## 2.1 Forma de execução
O projeto deve rodar com um único comando:
```bash
unzip challenge.zip
docker compose up
```
Não deve exigir configurações manuais adicionais.

## 2.2 Ambiente
Aplicação acessível em: [http://localhost:5174](http://localhost:5174)

# 3. Arquitetura e Containers

O sistema deve rodar com **3 containers Docker**:

## 3.1 Frontend
- **Stack:** SvelteKit, TypeScript, TailwindCSS e/ou DaisyUI.
- **Estrutura:** Seguir padrão do SvelteKit.
- **Adapter:** Usar `adapter-static`.
- **Foco:** Experiência do usuário (UX).

## 3.2 Backend
- **Stack:** .NET, C#.
- **Responsabilidade:** Lógica de negócios e persistência.

## 3.3 Banco de Dados
- **Stack:** MongoDB.
- **Persistência:** Dados descartáveis (sem volumes).

# 4. Funcionalidades da Aplicação

## 4.1 Compromissos
- Criar e excluir compromissos.
- Alterar cor dos compromissos.

## 4.2 Manipulação de datas
- Alterar data via **drag and drop**.
- Mover compromissos entre dias e semanas.

## 4.3 Navegação do calendário
- Visualização semanal.
- Componente lateral para seleção de data (date picker).

# 5. Testes Automatizados

## 5.1 Ferramenta
- **Playwright**

## 5.2 Requisitos dos testes
- Cobertura de testes obrigatória.
- **Modo:** Não-headless (navegador visível).

## 5.3 Execução obrigatória
```bash
npx playwright test browser.spec.ts
```

## 5.4 Configuração obrigatória (`playwright.config.ts`)
```typescript
import type { PlaywrightTestConfig } from '@playwright/test';
import { CandidateSelectionClient } from '../../src/services/CandidateSelectionClient';

const config: PlaywrightTestConfig = {
  webServer: {
    command: 'npm run build && npm run preview',
    port: 4173
  },
  testDir: 'tests',
  testMatch: /(.+.)?(test|spec).[jt]s/,
  use: {
    headless: false
  }
};

export default config;
```

# 6. Restrições de Código e Projeto

## 6.1 Idioma
Todo o código deve estar em **Inglês** (variáveis, funções, comentários, etc).

## 6.2 Estrutura e limpeza
O arquivo ZIP **NÃO** deve conter:
- `node_modules/`, `.git/`, `.svelte-kit/`, `.vscode/`, `build/`.

# 7. Arquivo de Entrega
- **Formato:** `challenge.zip`
- **Tamanho máximo:** 200 KB

# 8. Resumo Executivo (Checklist)

- [ ] 3 containers Docker (Front, Back, MongoDB)
- [ ] Executa com `docker compose up`
- [ ] Frontend em SvelteKit + TS + Tailwind/DaisyUI
- [ ] Backend em .NET + C#
- [ ] MongoDB sem persistência
- [ ] App tipo Google Calendar com Drag and drop
- [ ] Porta 5174 e `adapter-static`
- [ ] Código 100% em inglês
- [ ] Playwright com navegador visível
- [ ] ZIP ≤ 200 KB sem arquivos proibidos
```