# KSP Piloto Automático

Protótipo em C# para controlar o **Kerbal Space Program** por meio do [kRPC](https://krpc.github.io/krpc/). A proposta é experimentar automação de voo e telemetria a partir de um programa externo ao jogo.

## Estado atual

O repositório já contém a solução .NET e as dependências do cliente kRPC, mas a lógica de conexão e os controles de voo ainda não foram implementados. Ele é mantido como base de estudo para futuras rotinas de lançamento, orientação e manobra.

## Tecnologias

- C# / .NET 10
- [kRPC.Client](https://www.nuget.org/packages/KRPC.Client/) para comunicação com o jogo
- Google Protobuf, dependência do protocolo kRPC

## Como preparar o ambiente

1. Instale o Kerbal Space Program.
2. Instale e inicie o servidor kRPC no jogo.
3. Abra a solução `KSP-PilotoAutomatico.sln` em uma IDE compatível com .NET.
4. Restaure as dependências e execute o projeto:

```bash
dotnet restore KSP-PilotoAutomatico.sln
dotnet run --project KSP-PilotoAutomatico/KSP-PilotoAutomatico.csproj
```

## Próximos passos

- Conectar ao servidor kRPC e obter a nave ativa.
- Expor telemetria de altitude, velocidade e combustível.
- Implementar controle de aceleração, estágio e orientação.
- Criar rotinas reproduzíveis de lançamento e órbita.

## Licença

Nenhuma licença foi definida neste repositório.
