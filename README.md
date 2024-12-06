# Tech Challenge - 1ª Parte
Tech Challenge é o projeto da fase que engloba os conhecimentos obtidos em todas as disciplinas dela. Esta é uma atividade que, a princípio, deve ser desenvolvida em grupo. É importante atentar-se ao prazo de entrega, uma vez que essa atividade é obrigatória e vale 90% da nota de todas as disciplinas da fase.


## O Problema
O Tech Challenge desta fase será desenvolver um aplicativo utilizando a plataforma .NET 8 para cadastro de contatos regionais, considerando a persistência de dados e a qualidade do software.

### Requisitos Funcionais

- Cadastro de contatos: permitir o cadastro de novos contatos, incluindo nome, telefone e e-mail. Associe cada contato a um DDD correspondente à região. 
- Consulta de contatos: implementar uma funcionalidade para consultar e visualizar os contatos cadastrados, os quais podem ser filtrados pelo DDD da região. 
- Atualização e exclusão: possibilitar a atualização e a exclusão de contatos previamente cadastrados.

### Requisitos Técnicos

-  Persistência de Dados: utilizar um banco de dados para armazenar as informações dos contatos. Escolha entre Entity Framework Core ou Dapper para a camada de acesso a dados. 
- Cache: Implementação de cache no endpoint de consulta de contatos 
- Validações: implementar validações para garantir dados consistentes (por exemplo: validação de formato de e-mail, telefone, campos obrigatórios). 
- Testes Unitários: desenvolver testes unitários utilizando xUnit ou NUnit.

### Observações

O foco principal é a qualidade do código, as boas práticas de desenvolvimento e o uso eficiente da plataforma .NET 8. Este desafio é uma oportunidade para demonstrar habilidades em persistência de dados, arquitetura de software e testes, além de boas práticas de desenvolvimento. A entrega do frontend não é obrigatória. Apenas a documentação da API basta (swagger).

[![SonarQube Cloud](https://sonarcloud.io/images/project_badges/sonarcloud-light.svg)](https://sonarcloud.io/summary/new_code?id=grupo-tech-challenge_grupo-tech-challenge-v2)