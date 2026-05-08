# Maintenance Agent

## Responsabilidade

Analisa o histórico de quilometragem do veículo e determina quais manutenções
estão pendentes ou próximas do vencimento, com base nas regras padrão de
intervalo de manutenção.

## Entrada

- Lista de `VehicleRecord` (saída do CsvReaderAgent)
- Quilometragem atual do veículo

## Saída

- Lista de `MaintenanceSuggestion` com tipo de manutenção, urgência e descrição

## Regras de Manutenção Padrão

| Manutenção         | Intervalo (km) | Urgência ao atingir |
|--------------------|----------------|---------------------|
| Troca de Óleo      | 5.000 km       | Alta                |
| Rodízio de Pneus   | 10.000 km      | Média               |
| Filtro de Ar       | 15.000 km      | Média               |
| Revisão Geral      | 10.000 km      | Alta                |
| Velas de Ignição   | 30.000 km      | Média               |
| Troca de Pneus     | 40.000 km      | Alta                |

## Comportamento via IA (Semantic Kernel)

- Usa GPT-4o como modelo de linguagem
- Prompt do sistema define o papel de "especialista em manutenção automotiva"
- Envia os dados do veículo formatados como contexto
- Retorna análise em JSON estruturado
- Considera urgência: `Alta`, `Média`, `Baixa`
