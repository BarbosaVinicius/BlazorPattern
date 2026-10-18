# Arquitetura

## Visão geral

```
Página/Componente (Pages, Shared, Components)
        │  usa
        ▼
IPatternService  ── regras de negócio, validação, logs   (Services/PatternService.cs)
        │  usa
        ▼
IPatternRepository ── acesso a dados                      (Services/InMemoryPatternRepository.cs)
```

Cada camada depende apenas da **interface** da camada abaixo; o registro de DI está em
[Services/ServiceCollectionExtensions.cs](../Services/ServiceCollectionExtensions.cs).

| Item | Tempo de vida | Motivo |
|------|---------------|--------|
| `IPatternRepository` (em memória) | Singleton | O armazenamento precisa sobreviver a cada circuito Blazor. Um repositório com banco/`DbContext` deve ser **scoped**. |
| `IPatternService` | Scoped | Uma instância por circuito (ou por requisição, no prerender). |

## Renderização

O projeto usa o modelo **Blazor Web App** do .NET 8:

- **Estático (SSR)** é o padrão. `Routes`, `MainLayout` e `NavMenu` são estáticos: sem circuito, sem estado no servidor.
- **`@rendermode InteractiveServer`** é declarado **por página** apenas onde há eventos (`@onclick`, formulários):
  `Index`, `NewPage` e `Patterns`. Só então a conexão SignalR é aberta.
- `HeadOutlet` é mantido estático de propósito. Torná-lo interativo abriria um circuito em *todas* as páginas.
- Parâmetros com delegates/`RenderFragment` não atravessam a fronteira estático → interativo; por isso `Index` é
  interativa por inteiro, pois passa callbacks ao `StarterComponent`.

### Prerendering e `PersistentComponentState`

Páginas interativas são renderizadas primeiro no servidor (HTML rápido) e depois reiniciadas no circuito.
Sem cuidado, `OnInitializedAsync` roda duas vezes e a consulta é repetida (e a tela pisca). Em
[Pages/Patterns.razor](../Pages/Patterns.razor) os dados são gravados com `PersistAsJson` no prerender e lidos
com `TryTakeFromJson` na fase interativa.

### Tratamento de erros

- Exceções esperadas (`ValidationException`, `KeyNotFoundException`) são capturadas na página e exibidas ao usuário.
- `ErrorBoundary` em `MainLayout` protege páginas **estáticas**. Páginas interativas são raízes separadas (ilhas) e
  não ficam sob esse boundary; se precisar, envolva o conteúdo da própria página em um `ErrorBoundary`.
- Falhas não tratadas em produção vão para `/Error` (ver [Pages/Error.razor](../Pages/Error.razor)), que exibe só o
  *request id*. `DetailedErrors` fica ligado apenas em Development.

## Validação

As regras ficam no modelo ([Models/Pattern.cs](../Models/Pattern.cs)) via DataAnnotations. O `EditForm` as usa para
feedback imediato e o `PatternService` as reaplica com `Validator.ValidateObject`, garantindo as regras mesmo que a UI
seja contornada. Erros de validação saem como `ValidationException`.

## Contratos do serviço

| Método | Retorno | Falhas |
|--------|---------|--------|
| `GetAllAsync` | Lista ordenada por id | — |
| `GetByIdAsync(id)` | `Pattern` ou `null` | `ArgumentOutOfRangeException` (id ≤ 0) |
| `CreateAsync(item)` | Item com `Id` gerado | `ValidationException` |
| `UpdateAsync(item)` | Item atualizado | `ValidationException`, `KeyNotFoundException` |
| `DeleteAsync(id)` | — | `ArgumentOutOfRangeException`, `KeyNotFoundException` |

Todos aceitam `CancellationToken`. O repositório devolve **cópias** dos objetos para que a UI não altere o estado
armazenado por acidente.

## Componentes

- **`ModalDialog`**: só emite marcação quando visível; `role="dialog"`, `aria-modal`, `aria-labelledby`; fecha com
  Escape; suporta `@bind-IsVisible`. Deve ser usado dentro de componente interativo.
- **`StarterComponent`**: exemplo de composição (conteúdo filho + modal + callback).

## Otimizações aplicadas

- Circuito SignalR somente nas páginas que precisam.
- Prerender sem consulta duplicada (`PersistentComponentState`).
- `@key` nas linhas da tabela; modal fora da árvore enquanto fechado.
- Compressão de respostas (`UseResponseCompression`, sem HTTPS por padrão para evitar BREACH).
- `Nullable` e `ImplicitUsings` habilitados; documentação XML gerada.
- Health check em `/health`.

## Guia: trocar a persistência por EF Core

1. Adicione `Microsoft.EntityFrameworkCore.SqlServer` (ou outro provedor) e crie um `DbContext` com `DbSet<PatternEntity>`.
2. Implemente `IPatternRepository` convertendo `PatternEntity` ⇄ `Pattern`.
3. Registre `AddDbContext` e `AddScoped<IPatternRepository, EfPatternRepository>()` em `ServiceCollectionExtensions`.
4. Em páginas interativas de longa duração, prefira `IDbContextFactory<T>` a um `DbContext` scoped, pois o escopo dura
   o circuito inteiro.

## Guia: nova página

1. Crie `Pages/Minha.razor` com `@page "/minha"`.
2. Sem eventos? Deixe **sem** `@rendermode` (estático). Com eventos? Adicione `@rendermode InteractiveServer`.
3. Adicione `<PageTitle>` e um `<h1>` (usado por `FocusOnNavigate`).
4. Inclua o link em `Shared/NavMenu.razor`.
