PRAGMA defer_foreign_keys=TRUE;
CREATE TABLE tb_cardapio (
    id_cardapio INTEGER PRIMARY KEY AUTOINCREMENT, 
    nome_produto VARCHAR(70), 
    sabor VARCHAR(30), 
    preco DECIMAL(2,2), 
    peso_liquido VARCHAR(6), 
    ingredientes TEXT
    
, imagem text, frezzer int, tipo varchar (30), observacoes text, alergias varchar (30), pode_conter varchar (30), tamanho varchar (20), validade DATE, lote VARCHAR(50), em_promocao BOOLEAN DEFAULT 0, valor_promocional DECIMAL(10,2), aliquotas_imposto DECIMAL(10,2), ncm VARCHAR(20), cest VARCHAR(20), custo DECIMAL(10,2), unidade_venda VARCHAR(10), codigo_geral VARCHAR(100), codigo_barras VARCHAR(100), quantidade_minima DECIMAL(10,3) DEFAULT 0, quantidade DECIMAL(10,3) DEFAULT 0, linha text, categoria text, caseiro boolean);
INSERT INTO "tb_cardapio" ("id_cardapio","nome_produto","sabor","preco","peso_liquido","ingredientes","imagem","frezzer","tipo","observacoes","alergias","pode_conter","tamanho","validade","lote","em_promocao","valor_promocional","aliquotas_imposto","ncm","cest","custo","unidade_venda","codigo_geral","codigo_barras","quantidade_minima","quantidade","linha","categoria","caseiro") VALUES(1,'vksjvb','dfgbd',4,'123','1rthrnsxygnbsrgtjnsrtj',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,0,NULL,NULL,NULL);
INSERT INTO "tb_cardapio" ("id_cardapio","nome_produto","sabor","preco","peso_liquido","ingredientes","imagem","frezzer","tipo","observacoes","alergias","pode_conter","tamanho","validade","lote","em_promocao","valor_promocional","aliquotas_imposto","ncm","cest","custo","unidade_venda","codigo_geral","codigo_barras","quantidade_minima","quantidade","linha","categoria","caseiro") VALUES(2,'vksjvb','dfgbd','17,00','123','1rthrnsxygnbsrgtjnsrtj',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,0,NULL,NULL,NULL);
INSERT INTO "tb_cardapio" ("id_cardapio","nome_produto","sabor","preco","peso_liquido","ingredientes","imagem","frezzer","tipo","observacoes","alergias","pode_conter","tamanho","validade","lote","em_promocao","valor_promocional","aliquotas_imposto","ncm","cest","custo","unidade_venda","codigo_geral","codigo_barras","quantidade_minima","quantidade","linha","categoria","caseiro") VALUES(3,'vksjvb','dfgbd','17,00','123','1rthrnsxygnbsrgtjnsrtj',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,0,NULL,NULL,NULL);
INSERT INTO "tb_cardapio" ("id_cardapio","nome_produto","sabor","preco","peso_liquido","ingredientes","imagem","frezzer","tipo","observacoes","alergias","pode_conter","tamanho","validade","lote","em_promocao","valor_promocional","aliquotas_imposto","ncm","cest","custo","unidade_venda","codigo_geral","codigo_barras","quantidade_minima","quantidade","linha","categoria","caseiro") VALUES(4,'vksjvb','dfgbd','17,00','123','1rthrnsxygnbsrgtjnsrtj',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,0,NULL,NULL,NULL);
INSERT INTO "tb_cardapio" ("id_cardapio","nome_produto","sabor","preco","peso_liquido","ingredientes","imagem","frezzer","tipo","observacoes","alergias","pode_conter","tamanho","validade","lote","em_promocao","valor_promocional","aliquotas_imposto","ncm","cest","custo","unidade_venda","codigo_geral","codigo_barras","quantidade_minima","quantidade","linha","categoria","caseiro") VALUES(5,'vksjvb','morango','','123','1rthrnsxygnbsrgtjnsrtj','https://res.cloudinary.com/zcmbg7pc/image/upload/v1785867463/yv2z6zwqmtcobxdzpmef.jpg',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,0,NULL,NULL,NULL);
INSERT INTO "tb_cardapio" ("id_cardapio","nome_produto","sabor","preco","peso_liquido","ingredientes","imagem","frezzer","tipo","observacoes","alergias","pode_conter","tamanho","validade","lote","em_promocao","valor_promocional","aliquotas_imposto","ncm","cest","custo","unidade_venda","codigo_geral","codigo_barras","quantidade_minima","quantidade","linha","categoria","caseiro") VALUES(6,'Bananão','Banana',12,'123','1rthrnsxygnbsrgtjnsrtj',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,'Un',NULL,NULL,0,0,NULL,NULL,NULL);
INSERT INTO "tb_cardapio" ("id_cardapio","nome_produto","sabor","preco","peso_liquido","ingredientes","imagem","frezzer","tipo","observacoes","alergias","pode_conter","tamanho","validade","lote","em_promocao","valor_promocional","aliquotas_imposto","ncm","cest","custo","unidade_venda","codigo_geral","codigo_barras","quantidade_minima","quantidade","linha","categoria","caseiro") VALUES(7,'Sorvete do Bob Esponja','Banana',8.99,'150','leite condensado, creme de leite e leite integral, combinados com achocolatado ou frutas','https://res.cloudinary.com/zcmbg7pc/image/upload/v1785871173/lzmh3n8jcrcrg8kyoine.jpg',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,0,NULL,NULL,NULL);
INSERT INTO "tb_cardapio" ("id_cardapio","nome_produto","sabor","preco","peso_liquido","ingredientes","imagem","frezzer","tipo","observacoes","alergias","pode_conter","tamanho","validade","lote","em_promocao","valor_promocional","aliquotas_imposto","ncm","cest","custo","unidade_venda","codigo_geral","codigo_barras","quantidade_minima","quantidade","linha","categoria","caseiro") VALUES(8,'Sorvete artístico','Uva',15,'123','leite condensado, creme de leite e leite integral, combinados com achocolatado ou frutas','https://res.cloudinary.com/zcmbg7pc/image/upload/v1785933992/wihx80nbwpsoyp680sg0.ico',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,4,NULL,NULL,NULL);
INSERT INTO "tb_cardapio" ("id_cardapio","nome_produto","sabor","preco","peso_liquido","ingredientes","imagem","frezzer","tipo","observacoes","alergias","pode_conter","tamanho","validade","lote","em_promocao","valor_promocional","aliquotas_imposto","ncm","cest","custo","unidade_venda","codigo_geral","codigo_barras","quantidade_minima","quantidade","linha","categoria","caseiro") VALUES(9,'Sorvete ','Chocomenta',30,'123','leite condensado, creme de leite e leite integral, combinados com achocolatado ou frutas','https://res.cloudinary.com/zcmbg7pc/image/upload/v1785946568/aiviih8qpcfyg6cithnm.ico',1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,0,NULL,NULL,NULL);
INSERT INTO "tb_cardapio" ("id_cardapio","nome_produto","sabor","preco","peso_liquido","ingredientes","imagem","frezzer","tipo","observacoes","alergias","pode_conter","tamanho","validade","lote","em_promocao","valor_promocional","aliquotas_imposto","ncm","cest","custo","unidade_venda","codigo_geral","codigo_barras","quantidade_minima","quantidade","linha","categoria","caseiro") VALUES(10,'Morango galego','morango e chocolate branco',15,'123','leite condensado, creme de leite e leite integral, combinados com achocolatado ou frutas',NULL,4,'crepe',NULL,NULL,NULL,NULL,NULL,NULL,1,13,NULL,NULL,NULL,NULL,'Un',NULL,NULL,0,0,NULL,NULL,NULL);
INSERT INTO "tb_cardapio" ("id_cardapio","nome_produto","sabor","preco","peso_liquido","ingredientes","imagem","frezzer","tipo","observacoes","alergias","pode_conter","tamanho","validade","lote","em_promocao","valor_promocional","aliquotas_imposto","ncm","cest","custo","unidade_venda","codigo_geral","codigo_barras","quantidade_minima","quantidade","linha","categoria","caseiro") VALUES(11,'Picolé de nata','Nata',5,'123','leite condensado, creme de leite e leite integral, combinados com achocolatado ou frutas','https://res.cloudinary.com/zcmbg7pc/image/upload/v1786120947/tv4z6e4ez8pq4ufwsf1s.jpg',6,'picolé','Zero açúcar','leite','Nenhum',NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,0,NULL,NULL,NULL);
INSERT INTO "tb_cardapio" ("id_cardapio","nome_produto","sabor","preco","peso_liquido","ingredientes","imagem","frezzer","tipo","observacoes","alergias","pode_conter","tamanho","validade","lote","em_promocao","valor_promocional","aliquotas_imposto","ncm","cest","custo","unidade_venda","codigo_geral","codigo_barras","quantidade_minima","quantidade","linha","categoria","caseiro") VALUES(12,'Picolé de flocos','creme com flocos de chocolate',4.5,'123','leite condensado, creme de leite e leite integral, combinados com achocolatado ou frutas','https://res.cloudinary.com/zcmbg7pc/image/upload/v1786121177/j6bn54gslfwiodkilfvv.jpg',7,'picole','Contém lactose','leite','Nenhum',NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,0,NULL,NULL,NULL);
INSERT INTO "tb_cardapio" ("id_cardapio","nome_produto","sabor","preco","peso_liquido","ingredientes","imagem","frezzer","tipo","observacoes","alergias","pode_conter","tamanho","validade","lote","em_promocao","valor_promocional","aliquotas_imposto","ncm","cest","custo","unidade_venda","codigo_geral","codigo_barras","quantidade_minima","quantidade","linha","categoria","caseiro") VALUES(13,'Picolé de flocos','creme com flocos de chocolate',4.5,'123','leite condensado, creme de leite e leite integral, combinados com achocolatado ou frutas','https://res.cloudinary.com/zcmbg7pc/image/upload/v1786121229/icacxkmhmphekd4rm6fi.jpg',7,'picole','Contém lactose','leite','Nenhum',NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,0,NULL,NULL,NULL);
INSERT INTO "tb_cardapio" ("id_cardapio","nome_produto","sabor","preco","peso_liquido","ingredientes","imagem","frezzer","tipo","observacoes","alergias","pode_conter","tamanho","validade","lote","em_promocao","valor_promocional","aliquotas_imposto","ncm","cest","custo","unidade_venda","codigo_geral","codigo_barras","quantidade_minima","quantidade","linha","categoria","caseiro") VALUES(14,'Expresso','café',20,'123','água quente, pó de café',NULL,NULL,'cafe','Zero açúcar','leite','Nenhum',NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,'Un',NULL,NULL,0,0,'Cafezito','Aquecer',NULL);
INSERT INTO "tb_cardapio" ("id_cardapio","nome_produto","sabor","preco","peso_liquido","ingredientes","imagem","frezzer","tipo","observacoes","alergias","pode_conter","tamanho","validade","lote","em_promocao","valor_promocional","aliquotas_imposto","ncm","cest","custo","unidade_venda","codigo_geral","codigo_barras","quantidade_minima","quantidade","linha","categoria","caseiro") VALUES(15,'Latte','Leite achocolatado',17,'350','água quente, pó de café','https://res.cloudinary.com/zcmbg7pc/image/upload/v1786129712/bczntuxs9h8dngeapdoo.jpg',8,'cafe','Alto em açúcar adicionado','nenhum','Nenhum','',NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,0,NULL,NULL,NULL);
INSERT INTO "tb_cardapio" ("id_cardapio","nome_produto","sabor","preco","peso_liquido","ingredientes","imagem","frezzer","tipo","observacoes","alergias","pode_conter","tamanho","validade","lote","em_promocao","valor_promocional","aliquotas_imposto","ncm","cest","custo","unidade_venda","codigo_geral","codigo_barras","quantidade_minima","quantidade","linha","categoria","caseiro") VALUES(16,'Casquinha simples','Creme',5,'200','leite condensado, creme de leite e leite integral, combinados com achocolatado ou frutas','https://res.cloudinary.com/zcmbg7pc/image/upload/v1786563746/grcrto5oz3e5gdzohnmm.jpg','','casquinha','','leite','leite','P',NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,0,NULL,NULL,NULL);
INSERT INTO "tb_cardapio" ("id_cardapio","nome_produto","sabor","preco","peso_liquido","ingredientes","imagem","frezzer","tipo","observacoes","alergias","pode_conter","tamanho","validade","lote","em_promocao","valor_promocional","aliquotas_imposto","ncm","cest","custo","unidade_venda","codigo_geral","codigo_barras","quantidade_minima","quantidade","linha","categoria","caseiro") VALUES(17,'Casquinha simples','Creme',5,'200','leite condensado, creme de leite e leite integral, combinados com achocolatado ou frutas','https://res.cloudinary.com/zcmbg7pc/image/upload/v1786563747/xaindf4hbqmyl9myqxcp.jpg','','casquinha','','leite','leite','P',NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,0,NULL,NULL,NULL);
INSERT INTO "tb_cardapio" ("id_cardapio","nome_produto","sabor","preco","peso_liquido","ingredientes","imagem","frezzer","tipo","observacoes","alergias","pode_conter","tamanho","validade","lote","em_promocao","valor_promocional","aliquotas_imposto","ncm","cest","custo","unidade_venda","codigo_geral","codigo_barras","quantidade_minima","quantidade","linha","categoria","caseiro") VALUES(18,'Casquinha simples','Creme',5,'200','leite condensado, creme de leite e leite integral, combinados com achocolatado ou frutas','https://res.cloudinary.com/zcmbg7pc/image/upload/v1786563747/f4314d69yhufwivkks0r.jpg','','casquinha','','leite','leite','P',NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,0,NULL,NULL,NULL);
INSERT INTO "tb_cardapio" ("id_cardapio","nome_produto","sabor","preco","peso_liquido","ingredientes","imagem","frezzer","tipo","observacoes","alergias","pode_conter","tamanho","validade","lote","em_promocao","valor_promocional","aliquotas_imposto","ncm","cest","custo","unidade_venda","codigo_geral","codigo_barras","quantidade_minima","quantidade","linha","categoria","caseiro") VALUES(19,'Casquinha simples','Creme',5,'200','leite condensado, creme de leite e leite integral, combinados com achocolatado ou frutas','https://res.cloudinary.com/zcmbg7pc/image/upload/v1786563748/hr1qo8rd2vbtn37mpvkb.jpg','','casquinha','','leite','leite','P',NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,0,NULL,NULL,NULL);
INSERT INTO "tb_cardapio" ("id_cardapio","nome_produto","sabor","preco","peso_liquido","ingredientes","imagem","frezzer","tipo","observacoes","alergias","pode_conter","tamanho","validade","lote","em_promocao","valor_promocional","aliquotas_imposto","ncm","cest","custo","unidade_venda","codigo_geral","codigo_barras","quantidade_minima","quantidade","linha","categoria","caseiro") VALUES(20,'Casquinha de morango','morango',9,'123','leite condensado, creme de leite e leite integral, combinados com achocolatado ou frutas','https://res.cloudinary.com/zcmbg7pc/image/upload/v1786638746/uhv3nregcje0yqnonnhf.jpg',8,'casquinha','','leite','leite','Cascão',NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,0,NULL,NULL,NULL);
INSERT INTO "tb_cardapio" ("id_cardapio","nome_produto","sabor","preco","peso_liquido","ingredientes","imagem","frezzer","tipo","observacoes","alergias","pode_conter","tamanho","validade","lote","em_promocao","valor_promocional","aliquotas_imposto","ncm","cest","custo","unidade_venda","codigo_geral","codigo_barras","quantidade_minima","quantidade","linha","categoria","caseiro") VALUES(21,'crepe de morango galego',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,0,NULL,NULL,1);
INSERT INTO "tb_cardapio" ("id_cardapio","nome_produto","sabor","preco","peso_liquido","ingredientes","imagem","frezzer","tipo","observacoes","alergias","pode_conter","tamanho","validade","lote","em_promocao","valor_promocional","aliquotas_imposto","ncm","cest","custo","unidade_venda","codigo_geral","codigo_barras","quantidade_minima","quantidade","linha","categoria","caseiro") VALUES(22,'Sorvete Choco Menta','Choco Menta',15,NULL,NULL,NULL,3,NULL,NULL,NULL,NULL,NULL,NULL,NULL,1,12,NULL,NULL,NULL,NULL,'Un',NULL,'1234567890',2,6,NULL,NULL,NULL);
INSERT INTO "tb_cardapio" ("id_cardapio","nome_produto","sabor","preco","peso_liquido","ingredientes","imagem","frezzer","tipo","observacoes","alergias","pode_conter","tamanho","validade","lote","em_promocao","valor_promocional","aliquotas_imposto","ncm","cest","custo","unidade_venda","codigo_geral","codigo_barras","quantidade_minima","quantidade","linha","categoria","caseiro") VALUES(23,'boo','chocolate',68,'500ml',NULL,NULL,7,'sorvete',NULL,NULL,NULL,'Pote 500ml','2026-09-26',NULL,0,45,NULL,NULL,NULL,67,'Un',NULL,'85144',67,69,'Marrom bombom','Refrescar',NULL);
INSERT INTO "tb_cardapio" ("id_cardapio","nome_produto","sabor","preco","peso_liquido","ingredientes","imagem","frezzer","tipo","observacoes","alergias","pode_conter","tamanho","validade","lote","em_promocao","valor_promocional","aliquotas_imposto","ncm","cest","custo","unidade_venda","codigo_geral","codigo_barras","quantidade_minima","quantidade","linha","categoria","caseiro") VALUES(24,'Tadabaly',NULL,0.43,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,'Un',NULL,'70795',56,67,'Morena tropicana','Aquecer',NULL);
INSERT INTO "tb_cardapio" ("id_cardapio","nome_produto","sabor","preco","peso_liquido","ingredientes","imagem","frezzer","tipo","observacoes","alergias","pode_conter","tamanho","validade","lote","em_promocao","valor_promocional","aliquotas_imposto","ncm","cest","custo","unidade_venda","codigo_geral","codigo_barras","quantidade_minima","quantidade","linha","categoria","caseiro") VALUES(25,'Blue ice',NULL,1.8,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,'Un',NULL,'68719',1,1,'Azulzinho','Energizar',NULL);
INSERT INTO "tb_cardapio" ("id_cardapio","nome_produto","sabor","preco","peso_liquido","ingredientes","imagem","frezzer","tipo","observacoes","alergias","pode_conter","tamanho","validade","lote","em_promocao","valor_promocional","aliquotas_imposto","ncm","cest","custo","unidade_venda","codigo_geral","codigo_barras","quantidade_minima","quantidade","linha","categoria","caseiro") VALUES(26,'preto de couro pelica soft preto',NULL,67,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,'Un',NULL,'69942',1,1,'Gula','Saciar',NULL);
CREATE TABLE tb_buffet (
    id_buffet INTEGER PRIMARY KEY AUTOINCREMENT,
    categoria VARCHAR(20),
    sabor VARCHAR(50),
    tamanho VARCHAR(30)
);
INSERT INTO "tb_buffet" ("id_buffet","categoria","sabor","tamanho") VALUES(2,'sorvete','morango','');
INSERT INTO "tb_buffet" ("id_buffet","categoria","sabor","tamanho") VALUES(3,'sorvete','Uva','');
INSERT INTO "tb_buffet" ("id_buffet","categoria","sabor","tamanho") VALUES(4,'calda','morango','');
INSERT INTO "tb_buffet" ("id_buffet","categoria","sabor","tamanho") VALUES(5,'sorvete','Banana','');
INSERT INTO "tb_buffet" ("id_buffet","categoria","sabor","tamanho") VALUES(6,'tamanho','','Pote 300ml');
INSERT INTO "tb_buffet" ("id_buffet","categoria","sabor","tamanho") VALUES(7,'Doce','Chocolate Belga','Grande');
INSERT INTO "tb_buffet" ("id_buffet","categoria","sabor","tamanho") VALUES(8,'Salgado','Queijo','Medio');
CREATE TABLE tb_promocoes (id_promocao integer  primary key AUTOINCREMENT not null, 
data_inicio date, data_final date, valor_minimo decimal (3,2), segmento_produtos varchar (70),
limites varchar (100), nome_campanha varchar (200), descricao text, maxima_desconto
varchar (100), foto text, produtos_selecionados text);
INSERT INTO "tb_promocoes" ("id_promocao","data_inicio","data_final","valor_minimo","segmento_produtos","limites","nome_campanha","descricao","maxima_desconto","foto","produtos_selecionados") VALUES(1,'2026-08-01','2026-08-13',NULL,NULL,NULL,'Vera verão',NULL,NULL,NULL,NULL);
INSERT INTO "tb_promocoes" ("id_promocao","data_inicio","data_final","valor_minimo","segmento_produtos","limites","nome_campanha","descricao","maxima_desconto","foto","produtos_selecionados") VALUES(2,'2026-08-15','2026-08-28',NULL,'crepe',NULL,'Vera verão',NULL,NULL,NULL,NULL);
INSERT INTO "tb_promocoes" ("id_promocao","data_inicio","data_final","valor_minimo","segmento_produtos","limites","nome_campanha","descricao","maxima_desconto","foto","produtos_selecionados") VALUES(3,'2026-08-01','2026-08-10',NULL,'crepe',NULL,'Vera verão',NULL,'15%',NULL,NULL);
INSERT INTO "tb_promocoes" ("id_promocao","data_inicio","data_final","valor_minimo","segmento_produtos","limites","nome_campanha","descricao","maxima_desconto","foto","produtos_selecionados") VALUES(6,'2026-08-01','2026-08-12',NULL,'cafe',NULL,'Esfriou o clima, derreteu o preço!',NULL,'50',NULL,'Expresso, Latte');
INSERT INTO "tb_promocoes" ("id_promocao","data_inicio","data_final","valor_minimo","segmento_produtos","limites","nome_campanha","descricao","maxima_desconto","foto","produtos_selecionados") VALUES(8,NULL,'2026-10-31T14:12',NULL,'Energizar',NULL,'Arriba!','Na compra de duas paletas mexicanas garanta 50% de desconto na terceira',NULL,NULL,'Picolé de flocos,Picolé de flocos,Expresso');
INSERT INTO "tb_promocoes" ("id_promocao","data_inicio","data_final","valor_minimo","segmento_produtos","limites","nome_campanha","descricao","maxima_desconto","foto","produtos_selecionados") VALUES(9,NULL,'2026-10-29T14:36',NULL,'sorvete',NULL,'cjshdvcsdv','bsdgbsfgbngf','50%',NULL,'Sorvete do Bob Esponja,Sorvete artístico,Morango galego,Picolé de nata,preto de couro pelica soft preto');
CREATE TABLE tb_sessao_caixa (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    id_caixa INTEGER,
    id_usuario INTEGER,
    data_abertura DATETIME,
    valor_fundo_troco DECIMAL(10,2),
    data_fechamento DATETIME,
    status BOOLEAN,
    valor_fechamento DECIMAL(10,2),
    sincronizado BOOLEAN DEFAULT 0,
    FOREIGN KEY(id_caixa) REFERENCES tb_caixa(id),
    FOREIGN KEY(id_usuario) REFERENCES tb_usuarios(id)
);
INSERT INTO "tb_sessao_caixa" ("id","id_caixa","id_usuario","data_abertura","valor_fundo_troco","data_fechamento","status","valor_fechamento") VALUES(1,1,NULL,'2026-08-24 10:00:00',NULL,NULL,1,NULL);
INSERT INTO "tb_sessao_caixa" ("id","id_caixa","id_usuario","data_abertura","valor_fundo_troco","data_fechamento","status","valor_fechamento") VALUES(2,1,5,'2026-09-30 15:53:03',0,'2026-09-30 15:53:55',0,0);
INSERT INTO "tb_sessao_caixa" ("id","id_caixa","id_usuario","data_abertura","valor_fundo_troco","data_fechamento","status","valor_fechamento") VALUES(3,1,5,'2026-09-30 16:18:12',0,'2026-09-30 16:20:09',0,0);
INSERT INTO "tb_sessao_caixa" ("id","id_caixa","id_usuario","data_abertura","valor_fundo_troco","data_fechamento","status","valor_fechamento") VALUES(4,1,5,'2026-10-14 14:17:27',0,NULL,1,NULL);
INSERT INTO "tb_sessao_caixa" ("id","id_caixa","id_usuario","data_abertura","valor_fundo_troco","data_fechamento","status","valor_fechamento") VALUES(5,1,5,'2026-10-14 14:19:01',0,NULL,1,NULL);
INSERT INTO "tb_sessao_caixa" ("id","id_caixa","id_usuario","data_abertura","valor_fundo_troco","data_fechamento","status","valor_fechamento") VALUES(6,1,5,'2026-10-14 14:22:00',0,NULL,1,NULL);
INSERT INTO "tb_sessao_caixa" ("id","id_caixa","id_usuario","data_abertura","valor_fundo_troco","data_fechamento","status","valor_fechamento") VALUES(7,1,5,'2026-10-14 14:25:34',0,NULL,1,NULL);
CREATE TABLE tb_caixa (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    id_filial INTEGER,
    nome VARCHAR(30),
    status BOOLEAN,
    FOREIGN KEY(id_filial) REFERENCES tb_filiais(id)
);
INSERT INTO "tb_caixa" ("id","id_filial","nome","status") VALUES(1,1,'Caixa Principal',1);
CREATE TABLE tb_fornecedores (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    cnpj VARCHAR(14) UNIQUE,
    nome_juridico VARCHAR(150),
    nome_fantasia VARCHAR(150),
    inscricao_estadual VARCHAR(20),
    cep VARCHAR(8),
    logradouro VARCHAR(100),
    numero VARCHAR(10),
    complemento VARCHAR(60),
    bairro VARCHAR(60),
    cidade VARCHAR(100),
    cod_mun_ibge INTEGER,
    uf CHAR(2),
    telefone VARCHAR(15),
    email VARCHAR(100),
    status BOOLEAN
);
INSERT INTO "tb_fornecedores" ("id","cnpj","nome_juridico","nome_fantasia","inscricao_estadual","cep","logradouro","numero","complemento","bairro","cidade","cod_mun_ibge","uf","telefone","email","status") VALUES(1,'12345678000199','Fornecedor Alimentos LTDA','Alimentos SA','123456789','01310100','Av Paulista','1000','Sala 1','Bela Vista','São Paulo',3550308,'SP','11999990001','contato@alimentossa.com',1);
INSERT INTO "tb_fornecedores" ("id","cnpj","nome_juridico","nome_fantasia","inscricao_estadual","cep","logradouro","numero","complemento","bairro","cidade","cod_mun_ibge","uf","telefone","email","status") VALUES(2,'98765432000111','Embalagens Brasil LTDA','EmbalagensBR','987654321','20040020','Rua do Comercio','500','Galpao 3','Centro','Rio de Janeiro',3304557,'RJ','21988880002','vendas@embalagensbr.com',1);
CREATE TABLE tb_clientes (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    nome VARCHAR(100),
    cpf_cnpj VARCHAR(14) UNIQUE,
    email VARCHAR(100),
    telefone VARCHAR(15),
    cep VARCHAR(8),
    logradouro VARCHAR(100),
    numero VARCHAR(10),
    bairro VARCHAR(60),
    cidade VARCHAR(100),
    complemento VARCHAR(60),
    cod_mun_ibge INTEGER,
    uf CHAR(2)
);
INSERT INTO "tb_clientes" ("id","nome","cpf_cnpj","email","telefone","cep","logradouro","numero","bairro","cidade","complemento","cod_mun_ibge","uf") VALUES(1,'CLIENTE CONSUMIDOR DE TESTE','12345678909',NULL,NULL,NULL,'Avenida do Cliente','200','Centro','Carlos Barbosa',NULL,4304802,'RS');
CREATE TABLE tb_usuarios (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    nome VARCHAR(100),
    email VARCHAR(100) UNIQUE,
    senha VARCHAR(255),
    pin VARCHAR(255),
    nivel_acesso INTEGER
, id_filial INTEGER REFERENCES tb_filiais(id), id_google TEXT);
INSERT INTO "tb_usuarios" ("id","nome","email","senha","pin","nivel_acesso","id_filial","id_google") VALUES(1,'Charlene Borges','charleneb073@gmail.com',NULL,NULL,NULL,NULL,'109348291348718546451');
INSERT INTO "tb_usuarios" ("id","nome","email","senha","pin","nivel_acesso","id_filial","id_google") VALUES(2,'Larissa Gazoli','larissagazoli45@gmail.com',NULL,NULL,NULL,NULL,'116805590147496041396');
INSERT INTO "tb_usuarios" ("id","nome","email","senha","pin","nivel_acesso","id_filial","id_google") VALUES(3,'Larissa Gazoli','larissagazoli17@gmail.com',NULL,NULL,NULL,NULL,'108582861289817993456');
INSERT INTO "tb_usuarios" ("id","nome","email","senha","pin","nivel_acesso","id_filial","id_google") VALUES(4,'Guilherme','whyguiih@gmail.com','56',NULL,NULL,NULL,'111962262531433348177');
INSERT INTO "tb_usuarios" ("id","nome","email","senha","pin","nivel_acesso","id_filial","id_google") VALUES(5,'lari','lari@mail.co','12345','12345',1,1,NULL);
CREATE TABLE tb_filiais (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    cnpj VARCHAR(14) UNIQUE,
    nome_juridico VARCHAR(150),
    nome_fantasia VARCHAR(150),
    status BOOLEAN,
    inscricao_estadual VARCHAR(20),
    inscricao_municipal VARCHAR(20),
    crt TINYINT,
    caminho_certificado VARCHAR(255),
    senha_certificado VARCHAR(255),
    ambiente_nfe TINYINT,
    cep VARCHAR(8),
    logradouro VARCHAR(100),
    numero VARCHAR(10),
    complemento VARCHAR(50),
    bairro VARCHAR(60),
    cidade VARCHAR(100),
    cod_mun_ibge INTEGER,
    uf CHAR(2),
    telefone VARCHAR(15),
    email VARCHAR(100)
);
INSERT INTO "tb_filiais" ("id","cnpj","nome_juridico","nome_fantasia","status","inscricao_estadual","inscricao_municipal","crt","caminho_certificado","senha_certificado","ambiente_nfe","cep","logradouro","numero","complemento","bairro","cidade","cod_mun_ibge","uf","telefone","email") VALUES(1,'12345678000123','MINHA EMPRESA DE TESTE LTDA','EMPRESA TESTE',NULL,'123456789',NULL,1,NULL,NULL,2,NULL,'Rua da Empresa','100',NULL,'Centro','Carlos Barbosa',4304802,'RS',NULL,NULL);
CREATE TABLE tb_versao (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    versao VARCHAR(20),
    link_download VARCHAR(255),
    data_lancamento DATETIME DEFAULT CURRENT_TIMESTAMP
);
INSERT INTO "tb_versao" ("id","versao","link_download","data_lancamento") VALUES(1,'1.0.0','https://exemplo.com/download/v1.0.0','2026-01-15 10:00:00');
INSERT INTO "tb_versao" ("id","versao","link_download","data_lancamento") VALUES(2,'1.1.0','https://exemplo.com/download/v1.1.0','2026-03-20 14:30:00');
CREATE TABLE tb_entradas_xml (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    chave_nfe VARCHAR(44) UNIQUE,
    id_fornecedor INTEGER,
    data_entrada DATETIME,
    valor_total DECIMAL(10,2),
    id_filial INTEGER,
    FOREIGN KEY(id_fornecedor) REFERENCES tb_fornecedores(id),
    FOREIGN KEY(id_filial) REFERENCES tb_filiais(id)
);
CREATE TABLE tb_funcionarios (
  id int,
  id_usuario int,
  nome_completo varchar(80) not null,
  data_nascimento date,
  genero tinyint, 
  raca tinyint,
  estado_civil tinyint,
  nacionalidade text(40),
  naturalidade text(40),
  cpf text(11) not null,
  orgao_emissor text(50),
  email text(80),
  telefone text(11),
  contato_emergencia text(11),
  pcd boolean,
  escolaridade tinyint,
  formacao_academica text(40),
  logradouro text(60),
  numero int(4),
  bairro text(40),
  cidade text(60),
  cep text(8),
  complemento text,
  status boolean,
  primary key(id),
  foreign key (id_usuario) references tb_usuarios (id)
);
CREATE TABLE tb_funcionarios_complemento (
  id int,
  id_funcionario int,
  data_admissao date,
  tipo contrato tinyint,
  cargo text,
  nivel_senioridade tinyint,
  setor text,
  gestor int,
  tempo_empregado text,
  modelo_trabalho tinyint,
  escala_trabalho tinyint,
  salario_base real,
  tipo_remuneracao tinyint,
  banco text,
  agencia int,
  chave_pix text,
  centro_custo text,
  data_demissao date,
  tipo_demissao tinyint,
  motivo_demissao text,
  primary key (id),
  foreign key (id_funcionario) references tb_funcionarios (id),
  foreign key (gestor) references tb_funcionarios (id)
);
CREATE TABLE tb_trocas (
    id_troca INTEGER PRIMARY KEY AUTOINCREMENT,
    tipo_troca VARCHAR(15) CHECK(tipo_troca IN ('troca', 'devolucao')),
    data_troca DATETIME DEFAULT CURRENT_TIMESTAMP,
    id_vendedor INTEGER,
    produto_retornado VARCHAR(20),
    quantidade DECIMAL(10,3),
    FOREIGN KEY(produto_retornado) REFERENCES tb_produtos(codigo_barras) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY(id_vendedor) REFERENCES tb_usuarios(id)
);
CREATE TABLE tb_destinatario ( id_destinatario INTEGER PRIMARY KEY AUTOINCREMENT, id_emitindo TEXT NOT NULL, razao_social TEXT NOT NULL, rua TEXT NOT NULL, numero TEXT NOT NULL, cidade TEXT NOT NULL, estado TEXT, codigo_ibge TEXT NOT NULL, cep TEXT, bairro TEXT, complemento TEXT, observaco TEXT,incricao_estadual TEXT);
INSERT INTO "tb_destinatario" ("id_destinatario","id_emitindo","razao_social","rua","numero","cidade","estado","codigo_ibge","cep","bairro","complemento","observaco","incricao_estadual") VALUES(1,'116805590147496041396','Larissa Gazoli','R. Dr. Carlos barbosa, 570','570','Carlos Barbosa','','000000','95185-000','Centro','56','eu concordo','pessoa jurídica isenta');
INSERT INTO "tb_destinatario" ("id_destinatario","id_emitindo","razao_social","rua","numero","cidade","estado","codigo_ibge","cep","bairro","complemento","observaco","incricao_estadual") VALUES(2,'116805590147496041396','Larissa Gazoli','R. Dr. Carlos barbosa, 570','570','Carlos Barbosa','','000000','95185-000','Centro','56','eu concordo','pessoa jurídica isenta');
INSERT INTO "tb_destinatario" ("id_destinatario","id_emitindo","razao_social","rua","numero","cidade","estado","codigo_ibge","cep","bairro","complemento","observaco","incricao_estadual") VALUES(3,'116805590147496041396','Larissa Gazoli','R. Dr. Carlos barbosa, 570','570','Carlos Barbosa','','000000','95185-000','Centro','56','eu concordo','pessoa jurídica isenta');
INSERT INTO "tb_destinatario" ("id_destinatario","id_emitindo","razao_social","rua","numero","cidade","estado","codigo_ibge","cep","bairro","complemento","observaco","incricao_estadual") VALUES(4,'116805590147496041396','Larissa Gazoli','R. Dr. Carlos barbosa, 570','570','Carlos Barbosa','','000000','95185-000','Centro','56','eu concordo','pessoa física');
INSERT INTO "tb_destinatario" ("id_destinatario","id_emitindo","razao_social","rua","numero","cidade","estado","codigo_ibge","cep","bairro","complemento","observaco","incricao_estadual") VALUES(5,'108582861289817993456','','Avn. Independência','409','Garibaldi','','000000','95720-000','Centro','','','pessoa física');
CREATE TABLE tb_emitente ( id_emitente INTEGER PRIMARY KEY AUTOINCREMENT, IE TEXT  default '800/6412551', razao_social TEXT  default 'Michele Dalsochio Dipp', regime_tributario TEXT  DEFAULT 'Mei' CHECK(regime_tributario IN ('Mei','Simples Nacional','Lucro Presumido','presumido','outro')), outro TEXT, logradouro TEXT default ' Rua La Plata' , numero TEXT  default '638', bairro TEXT 'Jardim Botânico', cep TEXT default '90670-040', codigo_municipio TEXT NOT NULL DEFAULT '4314902 ', CNAE TEXT DEFAULT '32.99-0-06 Fabricação de velas, inclusive decorativas' , cnpj TEXT);
INSERT INTO "tb_emitente" ("id_emitente","IE","razao_social","regime_tributario","outro","logradouro","numero","bairro","cep","codigo_municipio","CNAE","cnpj") VALUES(1,'800/6412551','Michele Dalsochio Dipp','Mei',NULL,'Rua La Plata','638','Jardim Botânico','90670-040','4314902','32.99-0-06 Fabricação de velas, inclusive decorativas','12345678000190');
INSERT INTO "tb_emitente" ("id_emitente","IE","razao_social","regime_tributario","outro","logradouro","numero","bairro","cep","codigo_municipio","CNAE","cnpj") VALUES(2,'123456789','Empresa Teste LTDA','Simples Nacional',NULL,'Av Teste','100','Centro','90000-000','4314902','10.00-0-00 Teste','98765432000199');
CREATE TABLE tb_lucros (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    valor_arrecadado DECIMAL(10,2),
    valor_despesas DECIMAL(10,2),
    valor_lucro DECIMAL(10,2),
    data_registro DATETIME DEFAULT CURRENT_TIMESTAMP
);
INSERT INTO "tb_lucros" ("id","valor_arrecadado","valor_despesas","valor_lucro","data_registro") VALUES(1,34,0,34,'2026-09-11 12:21:47');
INSERT INTO "tb_lucros" ("id","valor_arrecadado","valor_despesas","valor_lucro","data_registro") VALUES(2,17,0,NULL,'2026-09-14 18:53:32');
INSERT INTO "tb_lucros" ("id","valor_arrecadado","valor_despesas","valor_lucro","data_registro") VALUES(3,5000,3000,2000,'2026-09-01 09:00:00');
INSERT INTO "tb_lucros" ("id","valor_arrecadado","valor_despesas","valor_lucro","data_registro") VALUES(4,7500,4200,3300,'2026-09-15 12:00:00');
CREATE TABLE tb_vendas (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    id_sessao_caixa INTEGER,
    id_cliente VARCHAR(255),
    data_hora DATETIME,
    subtotal DECIMAL(10,2),
    desconto DECIMAL(10,2),
    total DECIMAL(10,2),
    status VARCHAR(15) CHECK(status IN ('pago', 'cancelado', 'pendente')),
    chave_nfe VARCHAR(44), 
    meio_de_compra boolean,
    id_cupom VARCHAR(25),
    sincronizado BOOLEAN DEFAULT 0,
    FOREIGN KEY(id_sessao_caixa) REFERENCES tb_sessao_caixa(id)
);
INSERT INTO "tb_vendas" ("id","id_sessao_caixa","id_cliente","data_hora","subtotal","desconto","total","status","chave_nfe","meio_de_compra") VALUES(1,NULL,'116805590147496041396','2026-09-11 12:20:07',34,0,34,'pendente',NULL,NULL);
INSERT INTO "tb_vendas" ("id","id_sessao_caixa","id_cliente","data_hora","subtotal","desconto","total","status","chave_nfe","meio_de_compra") VALUES(2,NULL,'116805590147496041396','2026-09-11 12:21:28',34,0,34,'pendente',NULL,NULL);
INSERT INTO "tb_vendas" ("id","id_sessao_caixa","id_cliente","data_hora","subtotal","desconto","total","status","chave_nfe","meio_de_compra") VALUES(3,NULL,'116805590147496041396','2026-09-11 12:21:47',34,0,34,'pendente',NULL,NULL);
INSERT INTO "tb_vendas" ("id","id_sessao_caixa","id_cliente","data_hora","subtotal","desconto","total","status","chave_nfe","meio_de_compra") VALUES(4,NULL,'108582861289817993456','2026-09-14 18:53:32',17,5,22,'pendente',NULL,NULL);
INSERT INTO "tb_vendas" ("id","id_sessao_caixa","id_cliente","data_hora","subtotal","desconto","total","status","chave_nfe","meio_de_compra") VALUES(5,1,NULL,'2026-09-21 12:20:07',85,NULL,8528520,'pago',NULL,NULL);
CREATE TABLE tb_itens_venda (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    id_venda INTEGER,
    id_produto INTEGER,
    quantidade DECIMAL(10,3),
    preco_unitario DECIMAL(10,2),
    subtotal DECIMAL(10,2),
    FOREIGN KEY(id_venda) REFERENCES tb_vendas(id),
    FOREIGN KEY(id_produto) REFERENCES tb_cardapio(id_cardapio)
);
INSERT INTO "tb_itens_venda" ("id","id_venda","id_produto","quantidade","preco_unitario","subtotal") VALUES(1,3,15,2,17,34);
INSERT INTO "tb_itens_venda" ("id","id_venda","id_produto","quantidade","preco_unitario","subtotal") VALUES(2,4,1,1,'17,00',NULL);
CREATE TABLE tb_pagamentos (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    id_venda INTEGER,
    metodo VARCHAR(20) CHECK(metodo IN ('pix', 'dinheiro', 'cartão de crédito', 'cartão de débito')),
    valor DECIMAL(10,2),
    cod_autorizacao_tef VARCHAR(50),
    FOREIGN KEY(id_venda) REFERENCES tb_vendas(id)
);
CREATE TABLE tb_carrinho (
    id_carrinho INTEGER PRIMARY KEY AUTOINCREMENT, 
    produto VARCHAR(250) NOT NULL, 
    quant INTEGER, 
    usuario_id VARCHAR(255), /* Alterado para VARCHAR para caber o ID do Google */
    carrinho INTEGER DEFAULT 0,
  id_produto INTEGER, 
  foreign key(id_produto) references tb_cardapio(id_cardapio) on update cascade on delete cascade
);
INSERT INTO "tb_carrinho" ("id_carrinho","produto","quant","usuario_id","carrinho","id_produto") VALUES(1789384967992,'via_id',1,'116805590147496041396',1,2);
INSERT INTO "tb_carrinho" ("id_carrinho","produto","quant","usuario_id","carrinho","id_produto") VALUES(1789497561565,'via_id',2,'111962262531433348177',1,4);
INSERT INTO "tb_carrinho" ("id_carrinho","produto","quant","usuario_id","carrinho","id_produto") VALUES(1789557457946,'via_id',1,'109348291348718546451',1,2);
CREATE TABLE tb_feedback ( 
    id_feedback INTEGER PRIMARY KEY AUTOINCREMENT,
    nome TEXT NOT NULL,
    criado_em DATETIME DEFAULT CURRENT_TIMESTAMP,
  	avaliacao text, 
  nota decimal (1,1)
, id_produto INTEGER);
INSERT INTO "tb_feedback" ("id_feedback","nome","criado_em","avaliacao","nota","id_produto") VALUES(1,'vksjvb','2026-09-14 16:59:58','snsfgnfgn',5,NULL);
INSERT INTO "tb_feedback" ("id_feedback","nome","criado_em","avaliacao","nota","id_produto") VALUES(2,'vksjvb','2026-09-14 17:09:41','Eu achei ótimo.',4,NULL);
INSERT INTO "tb_feedback" ("id_feedback","nome","criado_em","avaliacao","nota","id_produto") VALUES(3,'Cliente Teste 1','2026-09-10 15:00:00','Excelente produto, muito saboroso!',4.5,1);
INSERT INTO "tb_feedback" ("id_feedback","nome","criado_em","avaliacao","nota","id_produto") VALUES(4,'Cliente Teste 2','2026-09-12 18:30:00','Qualidade boa, entrega rápida.',4,2);
DELETE FROM sqlite_sequence;
INSERT INTO "sqlite_sequence" ("name","seq") VALUES('tb_cardapio',26);
INSERT INTO "sqlite_sequence" ("name","seq") VALUES('tb_buffet',8);
INSERT INTO "sqlite_sequence" ("name","seq") VALUES('tb_promocoes',9);
INSERT INTO "sqlite_sequence" ("name","seq") VALUES('tb_filiais',1);
INSERT INTO "sqlite_sequence" ("name","seq") VALUES('tb_caixa',1);
INSERT INTO "sqlite_sequence" ("name","seq") VALUES('tb_sessao_caixa',7);
INSERT INTO "sqlite_sequence" ("name","seq") VALUES('tb_clientes',1);
INSERT INTO "sqlite_sequence" ("name","seq") VALUES('tb_destinatario',5);
INSERT INTO "sqlite_sequence" ("name","seq") VALUES('tb_vendas',5);
INSERT INTO "sqlite_sequence" ("name","seq") VALUES('tb_itens_venda',2);
INSERT INTO "sqlite_sequence" ("name","seq") VALUES('tb_lucros',4);
INSERT INTO "sqlite_sequence" ("name","seq") VALUES('tb_usuarios',5);
INSERT INTO "sqlite_sequence" ("name","seq") VALUES('tb_carrinho',1789557457946);
INSERT INTO "sqlite_sequence" ("name","seq") VALUES('tb_feedback',4);
INSERT INTO "sqlite_sequence" ("name","seq") VALUES('tb_fornecedores',2);
INSERT INTO "sqlite_sequence" ("name","seq") VALUES('tb_versao',2);
INSERT INTO "sqlite_sequence" ("name","seq") VALUES('tb_emitente',2);
