# PartsList Agent

## Responsabilidade

Com base nas sugestões de manutenção geradas pelo MaintenanceAgent, cria uma
lista detalhada de peças que precisam ser adquiridas antes de ir à oficina.

## Entrada

- Lista de `MaintenanceSuggestion` (saída do MaintenanceAgent)
- Nome/modelo do veículo

## Saída

- Lista de `Part` com nome da peça, quantidade, observações e prioridade

## Comportamento via IA (Semantic Kernel)

- Usa GPT-4o como modelo de linguagem
- Prompt do sistema define o papel de "especialista em peças automotivas"
- Recebe a lista de manutenções pendentes como contexto
- Retorna lista de peças em JSON estruturado com:
  - Nome da peça
  - Quantidade sugerida
  - Prioridade (Alta / Média / Baixa)
  - Observações (ex: "verifique a viscosidade recomendada pelo fabricante")

## Exemplo de saída esperada

```
Peças para Toyota Corolla (48.200 km):

[ALTA] Óleo do motor 5W-30 — 4 litros
[ALTA] Filtro de óleo — 1 unidade
[MÉDIA] Filtro de ar — 1 unidade
[MÉDIA] Velas de ignição — 4 unidades
```
