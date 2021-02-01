--TesteMC1-DB

DROP TABLE dbo.MovimentacoesItens
GO
DROP TABLE dbo.Movimentacoes
GO
DROP TABLE dbo.Produtos
GO
DROP TABLE dbo.Categorias
GO
DROP TABLE dbo.Usuarios 
GO



CREATE TABLE dbo.Usuarios 
    (
     Id BIGINT IDENTITY NOT NULL , 
     Nome VARCHAR (20) NOT NULL , 
     Sobrenome VARCHAR (100) NOT NULL , 
     Email VARCHAR (max) NOT NULL , 
     Senha VARCHAR (max) NOT NULL , 
     DataCriacao DATETIME NOT NULL , 
     CodigoAtivacao VARCHAR (6) , 
     DataCriacaoCodigoAtivacao DATETIME , 
     DataValidadeCodigoAtivacao DATETIME , 
     DataAtivacao DATETIME , 
     Perfil VARCHAR (30) NOT NULL , 
     EstaAtivo BIT NOT NULL 
    )
GO 
ALTER TABLE dbo.Usuarios ADD CONSTRAINT Usuarios_PK PRIMARY KEY CLUSTERED (Id)
GO
CREATE UNIQUE NONCLUSTERED INDEX 
    Usuarios_IDX1 ON dbo.Usuarios 
    ( 
     Email 
    ) 
GO



CREATE TABLE dbo.Categorias 
    (
     Id BIGINT IDENTITY NOT NULL , 
     Descricao VARCHAR (50) NOT NULL , 
     EstaAtiva BIT NOT NULL 
    )
GO
ALTER TABLE dbo.Categorias ADD CONSTRAINT Categorias_PK PRIMARY KEY CLUSTERED (Id)
GO


CREATE TABLE dbo.Produtos 
    (
     Id BIGINT IDENTITY NOT NULL , 
     Descricao VARCHAR (100) NOT NULL , 
     IdCategoria BIGINT NOT NULL , 
     CodigoInterno VARCHAR (50) , 
     CodigoBarras VARCHAR (50) , 
     UnidadeMedida VARCHAR (10) NOT NULL , 
     QtdEstoque DECIMAL (30,5) NOT NULL , 
     ValorUnitarioCusto DECIMAL (30,2) NOT NULL , 
     ValorUnitarioVenda DECIMAL (30,2) NOT NULL , 
     DataUltimaMovimentacao DATETIME , 
     EstaAtivo BIT NOT NULL 
    )
GO 
ALTER TABLE dbo.Produtos ADD CONSTRAINT Produtos_PK PRIMARY KEY CLUSTERED (Id)
GO
CREATE UNIQUE NONCLUSTERED INDEX 
    Produtos_IDX1 ON dbo.Produtos 
    ( 
     CodigoInterno 
    ) 
GO 
CREATE UNIQUE NONCLUSTERED INDEX 
    Produtos_IDX2 ON dbo.Produtos 
    ( 
     CodigoBarras 
    ) 
GO
ALTER TABLE dbo.Produtos 
    ADD CONSTRAINT Produtos_Categorias_FK FOREIGN KEY 
    ( 
     IdCategoria
    ) 
    REFERENCES dbo.Categorias 
    ( 
     Id 
    ) 
    ON DELETE NO ACTION 
    ON UPDATE NO ACTION 
GO



CREATE TABLE dbo.Movimentacoes 
    (
     Id BIGINT IDENTITY NOT NULL , 
     Data DATETIME NOT NULL , 
     Tipo VARCHAR (10) NOT NULL , 
     IdFornecedor BIGINT , 
     IdCliente BIGINT 
    )
GO
ALTER TABLE dbo.Movimentacoes ADD CONSTRAINT Movimentacao_PK PRIMARY KEY CLUSTERED (Id)
GO



CREATE TABLE dbo.MovimentacoesItens 
    (
     IdMovimentacao BIGINT NOT NULL , 
     NumeroItem INTEGER NOT NULL , 
     IdProduto BIGINT NOT NULL , 
     Quantidade DECIMAL (30,5) NOT NULL , 
     ValorUnitario DECIMAL (30,2) NOT NULL 
    )
GO
ALTER TABLE dbo.MovimentacoesItens ADD CONSTRAINT MovimentacaoItens_PK PRIMARY KEY CLUSTERED (IdMovimentacao, NumeroItem)
GO
ALTER TABLE dbo.MovimentacoesItens 
    ADD CONSTRAINT MovimentacoesItens_Movimentacoes_FK FOREIGN KEY 
    ( 
     IdMovimentacao
    ) 
    REFERENCES dbo.Movimentacoes 
    ( 
     Id 
    ) 
    ON DELETE NO ACTION 
    ON UPDATE NO ACTION 
GO
ALTER TABLE dbo.MovimentacoesItens 
    ADD CONSTRAINT MovimentacoesItens_Produtos_FK FOREIGN KEY 
    ( 
     IdProduto
    ) 
    REFERENCES dbo.Produtos 
    ( 
     Id 
    ) 
    ON DELETE NO ACTION 
    ON UPDATE NO ACTION 
GO
