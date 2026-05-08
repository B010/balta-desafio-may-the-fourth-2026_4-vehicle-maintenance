# CsvReader Agent

## Responsabilidade

Lê e parseia arquivos CSV contendo o histórico de quilometragem do veículo,
transformando os dados brutos em uma lista estruturada de registros.

## Entrada

- Caminho para o arquivo CSV
- Formato esperado: `Data,Quilometragem,Veiculo,Observacoes`

## Saída

- Lista de `VehicleRecord` com data, quilometragem, nome do veículo e observações

## Comportamento

- Não usa LLM — é um agente nativo (leitura e parsing puro)
- Valida o formato do CSV antes de processar
- Ordena os registros por data (mais antigo primeiro)
- Lança exceção descritiva se o arquivo não for encontrado ou estiver malformado

## Exemplo de CSV esperado

```csv
Data,Quilometragem,Veiculo,Observacoes
2024-01-15,45000,Toyota Corolla,Viagem longa
2024-02-20,46500,Toyota Corolla,Uso urbano
2024-03-10,48200,Toyota Corolla,Revisão agendada
```
