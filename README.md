# BlazorPattern

Projeto-modelo (template) de uma aplicação **Blazor Web App** em **.NET 8**, mostrando uma estrutura enxuta e otimizada:
renderização estática por padrão, interatividade apenas onde é necessária, camada de serviço, repositório
substituível, validação única e documentação em todo o código.

## Como executar

Requisitos: [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) ou superior.

```bash
dotnet run
```

Abra `https://localhost:5001` (perfil `BlazorPattern` em [Properties/launchSettings.json](Properties/launchSettings.json)).

| Rota        | Descrição                                                         |
|-------------|-------------------------------------------------------------------|
| `/`         | Página inicial com o modal de exemplo.                            |
| `/patterns` | CRUD completo de `Pattern` (listar, criar, editar, excluir).      |
| `/newpage`  | Destino do modal de exemplo.                                      |
| `/health`   | Health check (`200 Healthy`) para monitoramento/orquestradores.   |

## Estrutura

```
Program.cs                     Composition root (minimal hosting)
App.razor                      Documento HTML raiz (head, scripts)
Routes.razor                   Router
_Imports.razor                 Usings globais dos componentes
Pages/                         Páginas roteáveis (Index, Patterns, NewPage, Error)
Shared/                        Layout (MainLayout) e navegação (NavMenu)
Components/                    Componentes reutilizáveis (ModalDialog, StarterComponent)
Models/                        Modelos de domínio/UI com validação (Pattern)
Services/                      Regras de negócio e acesso a dados (IPatternService, IPatternRepository, ...)
Tables/                        Entidade de referência para persistência em banco (PatternEntity)
wwwroot/                       Arquivos estáticos (CSS, favicon)
docs/ARQUITETURA.md            Decisões de arquitetura e guias "como fazer"
```

## Principais recursos do Blazor utilizados

- **Modelo unificado do .NET 8** (`AddRazorComponents` + `MapRazorComponents`) com `blazor.web.js`.
- **Render mode por página** (`@rendermode InteractiveServer`): só `/`, `/patterns` e `/newpage` abrem circuito SignalR.
- **Prerendering + `PersistentComponentState`**: a lista de `/patterns` é carregada uma única vez.
- **Enhanced navigation** e `FocusOnNavigate` para navegação rápida e acessível.
- **`EditForm` + DataAnnotations** com a mesma validação reaplicada no serviço.
- **`ErrorBoundary`**, página `/Error` e `#blazor-error-ui` para tratamento de falhas.
- **`@key`** em listas, `PageTitle`/`HeadOutlet`, anti-forgery (`UseAntiforgery`) e compressão de respostas.

## Como estender

1. Crie o modelo em `Models/` com DataAnnotations.
2. Crie `I<Nome>Repository` + implementação e `I<Nome>Service` + implementação em `Services/`.
3. Registre em um método de extensão (veja `ServiceCollectionExtensions`) e chame-o em `Program.cs`.
4. Crie a página em `Pages/` e escolha o render mode (veja [docs/ARQUITETURA.md](docs/ARQUITETURA.md)).

## Persistência

Por padrão os dados ficam **em memória** (`InMemoryPatternRepository`) e são perdidos ao reiniciar. Para usar um banco,
implemente `IPatternRepository` (ex.: com EF Core, usando `Tables/PatternEntity.cs` como base) e troque o registro em
`ServiceCollectionExtensions`. Nenhuma outra camada precisa mudar.
