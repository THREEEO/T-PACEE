using Dapper;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace T_PACE
{
    public partial class MainWindow : Window
    {
        private ProdutoRepository _repositorio;
        public ObservableCollection<ItemCupom> Carrinho { get; set; }

        private decimal _descontoManualVenda = 0m;

        public MainWindow()
        {
            InitializeComponent();
            btnFinalizar.Click += (s, e) => AbrirTelaPagamento();
            txtBusca.KeyDown += TxtBusca_KeyDown;

            Task.Run(async () =>
            {
                await SincronizacaoService.SincronizarProdutosAsync();
                await SincronizacaoService.SincronizarVendasPendentesAsync();
            });

            // ATUALIZANDO O PERFIL VISUAL
            if (!string.IsNullOrEmpty(Session.CurrentUserName))
            {
                txtNomeUsuario.Text = Session.CurrentUserName;
                txtIniciaisUsuario.Text = Session.CurrentUserName.Length > 1
                    ? Session.CurrentUserName.Substring(0, 2).ToUpper()
                    : Session.CurrentUserName.ToUpper();
            }

            try
            {
                DatabaseConfig.InicializarBanco();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao criar banco local: {ex.Message}");
            }

            _repositorio = new ProdutoRepository();
            Carrinho = new ObservableCollection<ItemCupom>();

            listaCupom.ItemsSource = Carrinho;
            txtBusca.KeyDown += TxtBusca_KeyDown;

            Task.Run(async () => await SincronizacaoService.SincronizarProdutosAsync());
        }

        // Adicionamos a palavra 'async' aqui na assinatura do método
        private async void TxtBusca_KeyDown(object sender, KeyEventArgs e)
        {
            // Teclas de Atalho Globais
            if (e.Key == Key.F1)
            {
                AbrirTelaAjuda();
                e.Handled = true;
            }
            else if (e.Key == Key.F2)
            {
                AbrirTelaPagamento();
                e.Handled = true;
            }
            else if (e.Key == Key.F3)
            {
                AbrirTelaVendasDia();
                e.Handled = true;
            }
            else if (e.Key == Key.F4)
            {
                AbrirTelaItemAvulso();
                e.Handled = true;
            }
            else if (e.Key == Key.F5)
            {
                AbrirTelaBusca();
                e.Handled = true;
            }
            else if (e.Key == Key.F6)
            {
                AbrirTelaBaixaClientes();
                e.Handled = true;
            }
            else if (e.Key == Key.F7)
            {
                AbrirTelaMeuCaixa();
                e.Handled = true;
            }
            else if (e.Key == Key.F9)
            {
                MinimizarSistema();
                e.Handled = true;
            }
            else if (e.Key == Key.F10)
            {
                AbrirMenuMultifuncao();
                e.Handled = true;
            }
            else if (e.Key == Key.F11)
            {
                await EncerrarSistema();
                e.Handled = true;
            }
            else if (e.Key == Key.F12)
            {
                AbrirTelaPromocao();
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                FecharOverlayAberto();
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                string entradaCompleta = txtBusca.Text.Trim();
                if (!string.IsNullOrEmpty(entradaCompleta))
                {
                    ProcessarComandoLinha(entradaCompleta);
                }
                e.Handled = true;
            }
        }

        private void ProcessarComandoLinha(string entrada)
        {
            try
            {
                string entradaUpper = entrada.ToUpper().Trim();

                // COMANDOS ESPECIAIS
                if (entradaUpper == "NOVO")
                {
                    NovaVenda();
                    return;
                }

                // DESCONTO GLOBAL: Des[Valor] ou Des[%]
                if (entradaUpper.StartsWith("DES"))
                {
                    string valorDesconto = entrada.Substring(3).Trim();
                    if (!string.IsNullOrEmpty(valorDesconto))
                    {
                        txtValorDesconto.Text = valorDesconto;
                        AplicarDescontoManual();
                    }
                    else
                    {
                        AbrirTelaDesconto();
                    }
                    return;
                }

                // OBS: Complementa descrição do ÚLTIMO item
                if (entradaUpper.StartsWith("OBS "))
                {
                    string obs = entrada.Substring(4).Trim();
                    if (Carrinho.Count > 0 && !string.IsNullOrEmpty(obs))
                    {
                        var ultimoItem = Carrinho.Last();
                        ultimoItem.Descricao += " " + obs;
                        listaCupom.Items.Refresh();
                    }
                    txtBusca.Clear();
                    return;
                }

                // PARSE: quantidade*código ou quantidade x código
                int quantidadeDesejada = 1;
                string codigoFinal = entrada;
                string complementoDescricao = "";

                // Suporta tanto * quanto x/X como multiplicador
                char[] multiplicadores = { '*', 'x', 'X' };
                int idxMult = -1;
                foreach (char m in multiplicadores)
                {
                    idxMult = entrada.IndexOf(m);
                    if (idxMult >= 0) break;
                }

                if (idxMult > 0)
                {
                    string parteQtd = entrada.Substring(0, idxMult).Trim();
                    if (int.TryParse(parteQtd, out int qtd) && qtd > 0)
                    {
                        quantidadeDesejada = qtd;
                        codigoFinal = entrada.Substring(idxMult + 1).Trim();
                    }
                }

                // Verifica se há texto após o código (complemento de descrição)
                // Formato: código descrição (separado por espaço)
                string[] partesCodigo = codigoFinal.Split(new char[] { ' ' }, 2);
                if (partesCodigo.Length == 2)
                {
                    codigoFinal = partesCodigo[0].Trim();
                    complementoDescricao = partesCodigo[1].Trim();
                }

                // 2. BUSCA NO BANCO
                var produto = _repositorio.BuscarPorCodigoDeBarras(codigoFinal);

                if (produto != null)
                {
                    AdicionarProdutoAoCarrinho(produto, quantidadeDesejada, complementoDescricao);
                }
                else
                {
                    // Tenta buscar por ID numérico
                    if (int.TryParse(codigoFinal, out int idProduto))
                    {
                        produto = _repositorio.BuscarPorId(idProduto);
                        if (produto != null)
                        {
                            AdicionarProdutoAoCarrinho(produto, quantidadeDesejada, complementoDescricao);
                            return;
                        }
                    }

                    MessageBox.Show("Produto não cadastrado!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtBusca.SelectAll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro na busca: {ex.Message}", "Erro Crítico", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AdicionarProdutoAoCarrinho(Produto produto, int quantidade, string complementoDescricao)
        {
            decimal precoCheio = produto.preco_venda;
            decimal descontoDoItem = 0m;

            if (produto.em_promocao && produto.valor_promocional.HasValue)
            {
                decimal valorPromo = Convert.ToDecimal(produto.valor_promocional.Value);
                if (valorPromo > 0 && valorPromo < precoCheio)
                {
                    descontoDoItem = precoCheio - valorPromo;
                }
            }

            decimal precoCobrado = precoCheio - descontoDoItem;
            var itemExistente = Carrinho.FirstOrDefault(c => c.Codigo == produto.codigo_barras);

            if (itemExistente != null)
            {
                itemExistente.Quantidade += quantidade;
                itemExistente.Total = itemExistente.Quantidade * precoCobrado;
                if (!string.IsNullOrEmpty(complementoDescricao))
                {
                    itemExistente.Descricao += " " + complementoDescricao;
                }
            }
            else
            {
                Carrinho.Add(new ItemCupom
                {
                    IdProduto = produto.id,
                    Codigo = produto.codigo_barras,
                    Descricao = produto.nome + (string.IsNullOrEmpty(complementoDescricao) ? "" : " " + complementoDescricao),
                    Quantidade = quantidade,
                    PrecoUnitario = precoCheio,
                    DescontoUnitario = descontoDoItem,
                    Total = quantidade * precoCobrado
                });
            }

            txtUltimoNome.Text = produto.nome;
            txtUltimoDetalhes.Text = $"Cód: {produto.codigo_barras}  |  Qtd: {quantidade} un";
            txtUltimoPreco.Text = $"R$ {(quantidade * precoCobrado):N2}";

            AtualizarTotais();
            txtBusca.Clear();

            // --- CARREGAMENTO DA FOTO DO CACHE LOCAL INSTANTÂNEO ---
            if (!string.IsNullOrEmpty(produto.foto))
            {
                string nomeArquivo = produto.foto.Substring(produto.foto.LastIndexOf('/') + 1);
                string caminhoLocal = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Imagens", nomeArquivo);
                if (System.IO.File.Exists(caminhoLocal))
                {
                    try
                    {
                        var bitmap = new System.Windows.Media.Imaging.BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                        bitmap.UriSource = new Uri(caminhoLocal, UriKind.Absolute);
                        bitmap.EndInit();
                        bitmap.Freeze();

                        var rotatedBitmap = new System.Windows.Media.Imaging.TransformedBitmap(bitmap, new System.Windows.Media.RotateTransform(90));
                        rotatedBitmap.Freeze();

                        imgProdutoFoto.Source = rotatedBitmap;
                        imgProdutoFoto.Visibility = Visibility.Visible;
                        txtFotoPlaceholder.Visibility = Visibility.Collapsed;
                    }
                    catch
                    {
                        imgProdutoFoto.Visibility = Visibility.Collapsed;
                        txtFotoPlaceholder.Text = "ERRO NA FOTO";
                        txtFotoPlaceholder.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    imgProdutoFoto.Visibility = Visibility.Collapsed;
                    txtFotoPlaceholder.Text = "FOTO NÃO BAIXADA";
                    txtFotoPlaceholder.Visibility = Visibility.Visible;
                }
            }
            else
            {
                imgProdutoFoto.Visibility = Visibility.Collapsed;
                txtFotoPlaceholder.Text = "SEM FOTO";
                txtFotoPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void NovaVenda()
        {
            if (Carrinho.Count == 0) return;

            var result = MessageBox.Show("Iniciar nova venda? A venda atual será perdida.", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Carrinho.Clear();
                _descontoManualVenda = 0m;
                txtUltimoNome.Text = "Caixa Livre";
                txtUltimoDetalhes.Text = " ";
                txtUltimoPreco.Text = "R$ 0,00";
                imgProdutoFoto.Visibility = Visibility.Collapsed;
                txtFotoPlaceholder.Visibility = Visibility.Visible;
                AtualizarTotais();
            }
        }

        // ==========================================
        // LÓGICA DE CANCELAMENTO (F3)
        // ==========================================
        private void AbrirTelaCancelamento()
        {
            if (Carrinho.Count == 0)
            {
                MessageBox.Show("Não há itens no cupom para cancelar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            OverlayCancelamento.Visibility = Visibility.Visible;
            txtBuscaCancelamento.Clear();
            txtBuscaCancelamento.Focus();

            txtBuscaCancelamento.KeyDown -= TxtBuscaCancelamento_KeyDown;
            txtBuscaCancelamento.KeyDown += TxtBuscaCancelamento_KeyDown;
        }

        private void TxtBuscaCancelamento_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string codigo = txtBuscaCancelamento.Text.Trim();
                if (!string.IsNullOrEmpty(codigo))
                {
                    CancelarProduto(codigo);
                }
            }
            else if (e.Key == Key.Escape)
            {
                FecharTelaCancelamento();
            }
        }

        private void FecharTelaCancelamento()
        {
            OverlayCancelamento.Visibility = Visibility.Collapsed;
            txtBusca.Focus();
        }

        private void CancelarProduto(string codigo)
        {
            var itemExistente = Carrinho.FirstOrDefault(c => c.Codigo == codigo);

            if (itemExistente != null)
            {
                itemExistente.Quantidade -= 1;

                if (itemExistente.Quantidade <= 0)
                {
                    Carrinho.Remove(itemExistente);
                }
                else
                {
                    itemExistente.Total = itemExistente.Quantidade * (itemExistente.PrecoUnitario - itemExistente.DescontoUnitario);
                }

                txtUltimoNome.Text = "ITEM CANCELADO";
                txtUltimoDetalhes.Text = $"Cód: {codigo} removido";
                txtUltimoPreco.Text = $"- R$ {itemExistente.PrecoUnitario - itemExistente.DescontoUnitario:N2}";

                AtualizarTotais();
                FecharTelaCancelamento();
            }
            else
            {
                MessageBox.Show("Este item não está no cupom atual!", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtBuscaCancelamento.SelectAll();
            }
        }

// ==========================================
        // LÓGICA DE BUSCA POR NOME (F4)
        // ==========================================
        private void AbrirTelaBusca()
        {
            OverlayBusca.Visibility = Visibility.Visible;
            txtBuscaNome.Clear();
            listaResultadosBusca.ItemsSource = null;
            txtBuscaNome.Focus();
        }

        private void FecharTelaBusca()
        {
            OverlayBusca.Visibility = Visibility.Collapsed;
            txtBusca.Focus(); // Devolve o foco para o leitor de código de barras
        }

        // Pesquisa no banco enquanto o usuário digita
        private void txtBuscaNome_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string termo = txtBuscaNome.Text.Trim();

            // Só pesquisa a partir de 2 letras, para evitar sobrecarregar o banco
            if (termo.Length >= 2)
            {
                var resultados = _repositorio.BuscarPorNome(termo);
                listaResultadosBusca.ItemsSource = resultados;
            }
            else
            {
                listaResultadosBusca.ItemsSource = null;
            }
        }

        // Controle do teclado no campo de texto
        private void txtBuscaNome_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                FecharTelaBusca();
                e.Handled = true;
            }
            else if (e.Key == Key.Down && listaResultadosBusca.Items.Count > 0)
            {
                // Impede que o TextBox continue processando a seta
                e.Handled = true;

                // Move o foco para a lista e seleciona o primeiro item
                listaResultadosBusca.Focus();
                listaResultadosBusca.SelectedIndex = 0;

                // Força o "foco físico" no primeiro item da lista para as setas continuarem funcionando
                var item = (System.Windows.Controls.ListBoxItem)listaResultadosBusca.ItemContainerGenerator.ContainerFromIndex(0);
                item?.Focus();
            }
            else if (e.Key == Key.Enter && listaResultadosBusca.Items.Count > 0)
            {
                listaResultadosBusca.SelectedIndex = 0;
                SelecionarProdutoBusca();
                e.Handled = true;
            }
        }

        // Controle do teclado navegando dentro da lista
        private void listaResultadosBusca_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SelecionarProdutoBusca();
            }
            else if (e.Key == Key.Escape)
            {
                txtBuscaNome.Focus(); // Aperta Esc, devolve o foco pra digitar de novo
            }
        }

        // Para quem prefere usar o mouse (2 cliques)
        private void listaResultadosBusca_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelecionarProdutoBusca();
        }

        private void SelecionarProdutoBusca()
        {
            if (listaResultadosBusca.SelectedItem is Produto produtoSelecionado)
            {
                FecharTelaBusca();

                // Adiciona o código do produto ao lado do que já estiver no campo (ex: "5x")
                txtBusca.Text += produtoSelecionado.codigo_barras;

                // Joga o cursor piscante para o final do texto e devolve o foco
                txtBusca.CaretIndex = txtBusca.Text.Length;
                txtBusca.Focus();
            }
        }

        // ==========================================
        // LÓGICA DE DESCONTO GLOBAL (F5)
        // ==========================================
        private void AbrirTelaDesconto()
        {
            if (Carrinho.Count == 0)
            {
                MessageBox.Show("Adicione itens ao cupom antes de aplicar um desconto.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            OverlayDesconto.Visibility = Visibility.Visible;

            // Se já houver um desconto aplicado antes, mostra ele. Se não, limpa.
            txtValorDesconto.Text = _descontoManualVenda > 0 ? _descontoManualVenda.ToString("N2") : "";
            txtValorDesconto.SelectAll(); // Seleciona tudo para o operador sobrescrever fácil
            txtValorDesconto.Focus();
        }

        private void FecharTelaDesconto()
        {
            OverlayDesconto.Visibility = Visibility.Collapsed;
            txtBusca.Focus();
        }

        private void txtValorDesconto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                FecharTelaDesconto();
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                AplicarDescontoManual();
                e.Handled = true;
            }
        }

        private void AplicarDescontoManual()
        {
            string entrada = txtValorDesconto.Text.Trim();

            // Se o operador limpou o campo e deu Enter, removemos o desconto
            if (string.IsNullOrEmpty(entrada))
            {
                _descontoManualVenda = 0m;
                AtualizarTotais();
                FecharTelaDesconto();
                return;
            }

            // Descobre se o operador digitou o símbolo de %
            bool isPorcentagem = entrada.Contains("%");

            // Limpa a string para pegar só o número puro (tira R$, %, espaços e arruma a vírgula)
            entrada = entrada.Replace("R$", "").Replace("%", "").Replace(" ", "").Replace(",", ".");

            if (decimal.TryParse(entrada, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal valorDigitado))
            {
                // Descobre quanto a venda está custando agora (já abatendo os descontos de promoção)
                decimal subtotalBruto = Carrinho.Sum(i => i.PrecoUnitario * i.Quantidade);
                decimal descontosDePromocoes = Carrinho.Sum(i => i.DescontoUnitario * i.Quantidade);
                decimal totalAtualVenda = subtotalBruto - descontosDePromocoes;

                decimal valorDescontoFinal = 0m;

                if (isPorcentagem)
                {
                    // Regra de 3 básica: (Valor Total * Porcentagem) / 100
                    valorDescontoFinal = totalAtualVenda * (valorDigitado / 100m);
                }
                else
                {
                    // Se não tem %, é desconto em Reais direto
                    valorDescontoFinal = valorDigitado;
                }

                // Trava de segurança: O desconto não pode ser maior que a venda nem negativo
                if (valorDescontoFinal > totalAtualVenda || valorDescontoFinal < 0)
                {
                    MessageBox.Show("O desconto não pode ser negativo ou ultrapassar o valor total da venda!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtValorDesconto.SelectAll();
                    return;
                }

                // Aplica na variável global e atualiza a tela
                _descontoManualVenda = valorDescontoFinal;
                AtualizarTotais();
                FecharTelaDesconto();
            }
            else
            {
                MessageBox.Show("Valor inválido. Digite um número (ex: 10,50) ou uma porcentagem (ex: 15%).", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                txtValorDesconto.SelectAll();
            }
        }

        private void AtualizarTotais()
        {
            decimal subtotalBruto = Carrinho.Sum(i => i.PrecoUnitario * i.Quantidade);
            decimal descontosDePromocoes = Carrinho.Sum(i => i.DescontoUnitario * i.Quantidade);
            decimal descontosTotais = descontosDePromocoes + _descontoManualVenda;
            decimal totalAPagar = subtotalBruto - descontosTotais;

            txtSubtotal.Text = $"R$ {subtotalBruto:N2}";
            txtDesconto.Text = $"- R$ {descontosTotais:N2}";
            txtTotal.Text = $"R$ {totalAPagar:N2}";
        }

        private void txtBusca_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        // ==========================================
        // LÓGICA DE PAGAMENTO E FINALIZAÇÃO (F2)
        // ==========================================
        private void AbrirTelaPagamento()
        {
            if (Carrinho.Count == 0)
            {
                MessageBox.Show("Adicione itens ao cupom antes de finalizar a venda.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            decimal subtotalBruto = Carrinho.Sum(i => i.PrecoUnitario * i.Quantidade);
            decimal descontosDePromocoes = Carrinho.Sum(i => i.DescontoUnitario * i.Quantidade);
            decimal totalAPagar = subtotalBruto - (descontosDePromocoes + _descontoManualVenda);

            txtPagamentoTotal.Text = $"R$ {totalAPagar:N2}";
            txtValorRecebido.Text = totalAPagar.ToString("N2"); // Sugere o valor exato inicialmente
            txtTroco.Text = "R$ 0,00";

            OverlayPagamento.Visibility = Visibility.Visible;
            cmbMetodoPagamento.SelectedIndex = 0; // Reseta pro Dinheiro
            txtValorRecebido.SelectAll();
            txtValorRecebido.Focus();
        }

        private void FecharTelaPagamento()
        {
            OverlayPagamento.Visibility = Visibility.Collapsed;
            txtBusca.Focus();
        }

        private void txtValorRecebido_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            decimal subtotalBruto = Carrinho.Sum(i => i.PrecoUnitario * i.Quantidade);
            decimal descontosDePromocoes = Carrinho.Sum(i => i.DescontoUnitario * i.Quantidade);
            decimal totalAPagar = subtotalBruto - (descontosDePromocoes + _descontoManualVenda);

            string entrada = txtValorRecebido.Text.Trim().Replace("R$", "").Replace(" ", "").Replace(".", "").Replace(",", ".");

            if (decimal.TryParse(entrada, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal valorRecebido))
            {
                decimal troco = valorRecebido - totalAPagar;
                txtTroco.Text = troco < 0 ? "R$ 0,00" : $"R$ {troco:N2}";
            }
            else
            {
                txtTroco.Text = "R$ 0,00";
            }
        }

        private void txtValorRecebido_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                FecharTelaPagamento();
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                ProcessarVenda();
                e.Handled = true;
            }
            // NOVO: Setas para cima e para baixo trocam a forma de pagamento
            else if (e.Key == Key.Up)
            {
                // Se não estiver no primeiro item, sobe um. Se estiver, vai pro último.
                if (cmbMetodoPagamento.SelectedIndex > 0)
                    cmbMetodoPagamento.SelectedIndex--;
                else
                    cmbMetodoPagamento.SelectedIndex = cmbMetodoPagamento.Items.Count - 1;

                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                // Se não estiver no último item, desce um. Se estiver, vai pro primeiro.
                if (cmbMetodoPagamento.SelectedIndex < cmbMetodoPagamento.Items.Count - 1)
                    cmbMetodoPagamento.SelectedIndex++;
                else
                    cmbMetodoPagamento.SelectedIndex = 0;

                e.Handled = true;
            }
        }

        private void ProcessarVenda()
        {
            decimal subtotalBruto = Carrinho.Sum(i => i.PrecoUnitario * i.Quantidade);
            decimal descontosDePromocoes = Carrinho.Sum(i => i.DescontoUnitario * i.Quantidade);
            decimal descontoTotal = descontosDePromocoes + _descontoManualVenda;
            decimal totalAPagar = subtotalBruto - descontoTotal;

            string entrada = txtValorRecebido.Text.Trim().Replace("R$", "").Replace(" ", "").Replace(".", "").Replace(",", ".");
            decimal.TryParse(entrada, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal valorRecebido);
            if (valorRecebido < totalAPagar)
            {
                MessageBox.Show("O valor recebido é menor que o total da venda!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtValorRecebido.SelectAll();
                return;
            }

            // CALCULA O TROCO PARA SAIR NA NOTINHA
            decimal trocoFinal = valorRecebido - totalAPagar;
            // Se por acaso vir nulo, assume "dinheiro" como padrão
            string metodoSelecionado = ((System.Windows.Controls.ComboBoxItem)cmbMetodoPagamento.SelectedItem)?.Content?.ToString() ?? "dinheiro";

            // GERA CÓDIGO PAR DE 12 DÍGITOS (AnoMesDiaHoraMinutoSegundo)
            // Fica estreito na bobina, não precisa de zero extra (pois é par) e não se repete.
            string codigoUnicoCupom = DateTime.Now.ToString("yyMMddHHmmssff");

            try
            {
                using (var connection = new Microsoft.Data.Sqlite.SqliteConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();
                    // O BeginTransaction garante que as tabelas sejam salvas em lote (ou tudo funciona ou tudo cancela)
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // 1. Grava a Venda
                            // 1. Grava a Venda
                            var sqlVenda = @"INSERT INTO tb_vendas (id_sessao_caixa, id_cliente, data_hora, subtotal, desconto, total, status, id_cupom)
                                             VALUES (@Sessao, NULL, @DataHora, @Subtotal, @Desconto, @Total, 'pago', @IdCupom);
                                             SELECT last_insert_rowid();";

                            long idVenda = connection.ExecuteScalar<long>(sqlVenda, new
                            {
                                Sessao = Session.CurrentSessaoCaixaId,
                                DataHora = DateTime.Now,
                                Subtotal = subtotalBruto,
                                Desconto = descontoTotal,
                                Total = totalAPagar,
                                IdCupom = codigoUnicoCupom // SALVANDO NO BANCO LOCAL
                            }, transaction);

                            // 2. Grava os Itens e DÁ BAIXA NO ESTOQUE EM TODOS OS CÓDIGOS DE BARRAS IGUAIS
                            foreach (var item in Carrinho)
                            {
                                var sqlItem = @"INSERT INTO tb_itens_venda (id_venda, id_produto, quantidade, preco_unitario, subtotal)
                                                VALUES (@IdVenda, @IdProduto, @Quantidade, @PrecoUnitario, @Subtotal);
                                                
                                                UPDATE tb_produtos 
                                                SET quantidade = quantidade - @Quantidade 
                                                WHERE (codigo_geral IS NOT NULL AND codigo_geral = (SELECT codigo_geral FROM tb_produtos WHERE id = @IdProduto))
                                                   OR (codigo_geral IS NULL AND id = @IdProduto);";

                                connection.Execute(sqlItem, new
                                {
                                    IdVenda = idVenda,
                                    IdProduto = item.IdProduto,
                                    Quantidade = item.Quantidade,
                                    PrecoUnitario = item.PrecoUnitario,
                                    Subtotal = item.Total
                                }, transaction);
                            }

                            // 3. Grava o Pagamento

                            var sqlPagamento = @"INSERT INTO tb_pagamentos (id_venda, metodo, valor)
                                                 VALUES (@IdVenda, @Metodo, @Valor);";

                            connection.Execute(sqlPagamento, new
                            {
                                IdVenda = idVenda,
                                Metodo = metodoSelecionado,
                                Valor = totalAPagar
                            }, transaction);


                            transaction.Commit(); // Se chegou aqui, joga tudo pro arquivo do banco de vez!

                            // ==========================================
                            // IMPRIMIR O CUPOM DA VENDA
                            // ==========================================
                            ImprimirRecibo(idVenda, codigoUnicoCupom, Carrinho.ToList(), subtotalBruto, descontoTotal, totalAPagar, metodoSelecionado, valorRecebido, trocoFinal);

                            // CHAMA O SINCRONIZADOR DE FILA
                            Task.Run(async () => await SincronizacaoService.SincronizarVendasPendentesAsync());
                        }
                        catch (Exception)
                        {
                            transaction.Rollback(); // Deu erro? Cancela tudo pra não quebrar o banco
                            throw;
                        }
                    }
                }

                // Finalizou com sucesso! Limpa a tela pro próximo cliente
                Carrinho.Clear();
                _descontoManualVenda = 0m;
                txtUltimoNome.Text = "Caixa Livre";
                txtUltimoDetalhes.Text = " ";
                txtUltimoPreco.Text = "R$ 0,00";
                imgProdutoFoto.Visibility = Visibility.Collapsed;
                txtFotoPlaceholder.Visibility = Visibility.Visible;
                AtualizarTotais();
                FecharTelaPagamento();
            }
            catch (Exception ex)
            {
                // AGORA SIM ESTAMOS USANDO O "ex.Message"!
                // Se der erro ao vender, ele vai te falar exatamente o que quebrou!
                MessageBox.Show($"Erro ao concluir venda. Detalhe: {ex.Message}", "Erro Crítico", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ==========================================
        // LÓGICA DE SAIR DO CAIXA (F9) E FECHAMENTO
        // ==========================================
        private void AbrirTelaFechamento()
        {
            OverlayFechamento.Visibility = Visibility.Visible;
            txtValorFechamento.Text = "0,00";
            txtValorFechamento.SelectAll();
            txtValorFechamento.Focus();
        }

        private void FecharTelaFechamento()
        {
            OverlayFechamento.Visibility = Visibility.Collapsed;
            txtBusca.Focus();
        }

        private async void txtValorFechamento_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                FecharTelaFechamento();
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                e.Handled = true;
                string entrada = txtValorFechamento.Text.Trim().Replace("R$", "").Replace(" ", "").Replace(".", "").Replace(",", ".");
                decimal.TryParse(entrada, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal valorFechamento);

                // Grava o fechamento no banco de dados local
                using (var conn = new Microsoft.Data.Sqlite.SqliteConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    conn.Execute("UPDATE tb_sessao_caixa SET data_fechamento = @Data, status = 0, valor_fechamento = @Valor, sincronizado = 0 WHERE id = @Id",
                        new { Data = DateTime.Now, Valor = valorFechamento, Id = Session.CurrentSessaoCaixaId });
                }

                FecharTelaFechamento();
                await SairDoCaixa(); // Usa o método original para limpar a tela e deslogar
            }
        }

        private async Task SairDoCaixa()
        {
            // Bloqueia a tela e avisa o operador
            this.IsEnabled = false;
            txtUltimoNome.Text = "Encerrando Turno...";
            txtUltimoDetalhes.Text = "Aguarde enquanto os dados são salvos na nuvem.";
            txtUltimoPreco.Text = "";

            // Força o envio das vendas e SESSÕES DE CAIXA pendentes
            await SincronizacaoService.SincronizarVendasPendentesAsync();
            await SincronizacaoService.SincronizarSessoesCaixaAsync();

            // Limpa a Sessão de quem estava logado
            Session.CurrentUserId = 0;
            Session.CurrentUserName = string.Empty;
            Session.CurrentSessaoCaixaId = 0;

            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            this.Hide();

            var login = new LoginWindow();
            bool? ok = login.ShowDialog();

            if (ok == true)
            {
                var novaMain = new MainWindow();
                Application.Current.MainWindow = novaMain;
                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                novaMain.Show();
            }
            else
            {
                Application.Current.Shutdown();
            }
            this.Close();
        }

        // ==========================================
        // NOVAS TELAS DE ATALHO (F1, F3, F4, F6, F7, F10, F11, F12)
        // ==========================================

        private void FecharOverlayAberto()
        {
            if (OverlayCancelamento.Visibility == Visibility.Visible) FecharTelaCancelamento();
            else if (OverlayBusca.Visibility == Visibility.Visible) FecharTelaBusca();
            else if (OverlayDesconto.Visibility == Visibility.Visible) FecharTelaDesconto();
            else if (OverlayPagamento.Visibility == Visibility.Visible) FecharTelaPagamento();
            else if (OverlayFechamento.Visibility == Visibility.Visible) FecharTelaFechamento();
            else if (OverlayAjuda.Visibility == Visibility.Visible) FecharTelaAjuda();
            else if (OverlayVendasDia.Visibility == Visibility.Visible) FecharTelaVendasDia();
            else if (OverlayItemAvulso.Visibility == Visibility.Visible) FecharTelaItemAvulso();
            else if (OverlayBaixaClientes.Visibility == Visibility.Visible) FecharTelaBaixaClientes();
            else if (OverlayMeuCaixa.Visibility == Visibility.Visible) FecharTelaMeuCaixa();
            else if (OverlayMenuMultifuncao.Visibility == Visibility.Visible) FecharMenuMultifuncao();
            else if (OverlayPromocao.Visibility == Visibility.Visible) FecharTelaPromocao();
            else if (OverlayEncerrar.Visibility == Visibility.Visible) FecharTelaEncerrar();
        }

        private void MinimizarSistema()
        {
            this.WindowState = WindowState.Minimized;
        }

        private async Task EncerrarSistema()
        {
            var result = MessageBox.Show("Encerrar o sistema completamente?", "Confirmar Encerramento", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        // F1 - AJUDA
        private void AbrirTelaAjuda()
        {
            OverlayAjuda.Visibility = Visibility.Visible;
            // Foco no botão fechar ou similar
        }

        private void FecharTelaAjuda()
        {
            OverlayAjuda.Visibility = Visibility.Collapsed;
            txtBusca.Focus();
        }

        // F3 - VENDAS DO DIA
        private void AbrirTelaVendasDia()
        {
            CarregarVendasDoDia();
            OverlayVendasDia.Visibility = Visibility.Visible;
            OverlayVendasDia.Focus();
        }

        private void FecharTelaVendasDia()
        {
            OverlayVendasDia.Visibility = Visibility.Collapsed;
            txtBusca.Focus();
        }

        private void CarregarVendasDoDia()
        {
            using (var conn = new Microsoft.Data.Sqlite.SqliteConnection(DatabaseConfig.ConnectionString))
            {
                conn.Open();
                var vendas = conn.Query<VendaResumo>(@"
                    SELECT v.id, v.id_cupom, v.data_hora, v.subtotal, v.desconto, v.total, v.status,
                           COALESCE(SUM(CASE WHEN p.metodo = 'dinheiro' THEN p.valor END), 0) as TotalDinheiro,
                           COALESCE(SUM(CASE WHEN p.metodo = 'credito' OR p.metodo = 'cartão de crédito' THEN p.valor END), 0) as TotalCredito,
                           COALESCE(SUM(CASE WHEN p.metodo = 'debito' OR p.metodo = 'cartão de débito' THEN p.valor END), 0) as TotalDebito,
                           COALESCE(SUM(CASE WHEN p.metodo = 'pix' THEN p.valor END), 0) as TotalPix,
                           COALESCE(SUM(CASE WHEN p.metodo = 'crediario' THEN p.valor END), 0) as TotalCrediario,
                           COALESCE(SUM(CASE WHEN p.metodo NOT IN ('dinheiro','credito','cartão de crédito','debito','cartão de débito','pix','crediario') THEN p.valor END), 0) as TotalOutros
                    FROM tb_vendas v
                    LEFT JOIN tb_pagamentos p ON v.id = p.id_venda
                    WHERE date(v.data_hora) = date('now') AND v.status = 'pago'
                    GROUP BY v.id
                    ORDER BY v.data_hora DESC").ToList();

                listaVendasDia.ItemsSource = vendas;
            }
        }

        private void OverlayVendasDia_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                FecharTelaVendasDia();
                e.Handled = true;
            }
        }

        private void listaVendasDia_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listaVendasDia.SelectedItem is VendaResumo venda)
            {
                AbrirDetalhesVenda(venda);
            }
        }

        private void listaVendasDia_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && listaVendasDia.SelectedItem is VendaResumo venda)
            {
                AbrirDetalhesVenda(venda);
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                FecharTelaVendasDia();
                e.Handled = true;
            }
        }

        private void txtVendasDiaBusca_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FiltrarVendasDia(txtVendasDiaBusca.Text.Trim());
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                txtVendasDiaBusca.Clear();
                FiltrarVendasDia("");
                e.Handled = true;
            }
            else if (e.Key == Key.F5)
            {
                FiltrarVendasDia(txtVendasDiaBusca.Text.Trim());
                e.Handled = true;
            }
        }

        private void FiltrarVendasDia(string termo)
        {
            if (string.IsNullOrWhiteSpace(termo))
            {
                CarregarVendasDoDia();
                return;
            }

            using (var conn = new Microsoft.Data.Sqlite.SqliteConnection(DatabaseConfig.ConnectionString))
            {
                conn.Open();
                var vendas = conn.Query<VendaResumo>(@"
                    SELECT v.id, v.id_cupom, v.data_hora, v.subtotal, v.desconto, v.total, v.status,
                           COALESCE(SUM(CASE WHEN p.metodo = 'dinheiro' THEN p.valor END), 0) as TotalDinheiro,
                           COALESCE(SUM(CASE WHEN p.metodo = 'credito' OR p.metodo = 'cartão de crédito' THEN p.valor END), 0) as TotalCredito,
                           COALESCE(SUM(CASE WHEN p.metodo = 'debito' OR p.metodo = 'cartão de débito' THEN p.valor END), 0) as TotalDebito,
                           COALESCE(SUM(CASE WHEN p.metodo = 'pix' THEN p.valor END), 0) as TotalPix,
                           COALESCE(SUM(CASE WHEN p.metodo = 'crediario' THEN p.valor END), 0) as TotalCrediario,
                           COALESCE(SUM(CASE WHEN p.metodo NOT IN ('dinheiro','credito','cartão de crédito','debito','cartão de débito','pix','crediario') THEN p.valor END), 0) as TotalOutros
                    FROM tb_vendas v
                    LEFT JOIN tb_pagamentos p ON v.id = p.id_venda
                    WHERE date(v.data_hora) = date('now') AND v.status = 'pago'
                      AND (v.id_cupom LIKE @Termo OR CAST(v.id AS TEXT) LIKE @Termo)
                    GROUP BY v.id
                    ORDER BY v.data_hora DESC", new { Termo = $"%{termo}%" }).ToList();

                listaVendasDia.ItemsSource = vendas;
            }
        }

        private void AbrirDetalhesVenda(VendaResumo venda)
        {
            using (var conn = new Microsoft.Data.Sqlite.SqliteConnection(DatabaseConfig.ConnectionString))
            {
                conn.Open();
                var itens = conn.Query<ItemVendaDetalhe>(@"
                    SELECT iv.id, p.nome, iv.quantidade, iv.preco_unitario, iv.subtotal
                    FROM tb_itens_venda iv
                    JOIN tb_produtos p ON iv.id_produto = p.id
                    WHERE iv.id_venda = @IdVenda", new { IdVenda = venda.id }).ToList();

                var pagamentos = conn.Query<PagamentoDetalhe>(@"
                    SELECT metodo, valor
                    FROM tb_pagamentos
                    WHERE id_venda = @IdVenda", new { IdVenda = venda.id }).ToList();

                var detalhes = new VendaDetalheCompleto
                {
                    Venda = venda,
                    Itens = itens,
                    Pagamentos = pagamentos
                };

                // Show detail overlay
                MostrarDetalhesVenda(detalhes);
            }
        }

        private void MostrarDetalhesVenda(VendaDetalheCompleto detalhes)
        {
            // Preenche o overlay de detalhes existente
            txtDetalheVendaCupomDetalhe.Text = detalhes.Venda.id_cupom;
            txtDetalheVendaData.Text = detalhes.Venda.data_hora.ToString("dd/MM/yyyy HH:mm:ss");
            txtDetalheVendaStatus.Text = detalhes.Venda.status;
            txtDetalheVendaSubtotal.Text = $"R$ {detalhes.Venda.subtotal:N2}";
            txtDetalheVendaDesconto.Text = $"R$ {detalhes.Venda.desconto:N2}";
            txtDetalheVendaTotal.Text = $"R$ {detalhes.Venda.total:N2}";

            listaDetalheItens.ItemsSource = detalhes.Itens;
            listaDetalhePagamentos.ItemsSource = detalhes.Pagamentos;

            OverlayDetalhesVenda.Visibility = Visibility.Visible;
            OverlayDetalhesVenda.Focus();
        }

        private void FecharDetalhesVenda()
        {
            OverlayDetalhesVenda.Visibility = Visibility.Collapsed;
            OverlayVendasDia.Focus(); // Volta foco para a lista de vendas
        }

        private void OverlayDetalhesVenda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                FecharDetalhesVenda();
                e.Handled = true;
            }
        }

        private void FecharDetalhesVenda_Click(object sender, RoutedEventArgs e) => FecharDetalhesVenda();

        // F4 - ITEM AVULSO
        private void AbrirTelaItemAvulso()
        {
            txtItemAvulsoDesc.Clear();
            txtItemAvulsoComp.Clear();
            txtItemAvulsoQtd.Text = "1";
            txtItemAvulsoPreco.Clear();
            txtItemAvulsoTotal.Text = "R$ 0,00";
            OverlayItemAvulso.Visibility = Visibility.Visible;
            OverlayItemAvulso.Focus(); // Foco no overlay para KeyDown funcionar
            txtItemAvulsoDesc.Focus();
        }

        private void FecharTelaItemAvulso()
        {
            OverlayItemAvulso.Visibility = Visibility.Collapsed;
            txtBusca.Focus();
        }

        private void OverlayItemAvulso_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                FecharTelaItemAvulso();
                e.Handled = true;
            }
        }

        private void txtItemAvulso_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) { FecharTelaItemAvulso(); e.Handled = true; }
            else if (e.Key == Key.Enter) { ConfirmarItemAvulso_Click(null, null); e.Handled = true; }
            else if (e.Key == Key.Down) { MoveFocusNext(); e.Handled = true; }
            else if (e.Key == Key.Up) { MoveFocusPrev(); e.Handled = true; }
        }

        private void txtItemAvulso_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (txtItemAvulsoQtd == null || txtItemAvulsoPreco == null || txtItemAvulsoTotal == null) return;

            if (decimal.TryParse(txtItemAvulsoQtd.Text, out decimal qtd) &&
                decimal.TryParse(txtItemAvulsoPreco.Text.Replace("R$", "").Replace(" ", "").Replace(",", "."), out decimal preco))
            {
                txtItemAvulsoTotal.Text = $"R$ {(qtd * preco):N2}";
            }
            else
            {
                txtItemAvulsoTotal.Text = "R$ 0,00";
            }
        }

        private void ConfirmarItemAvulso_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtItemAvulsoDesc.Text))
            {
                MessageBox.Show("Informe a descrição do item.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(txtItemAvulsoQtd.Text, out decimal qtd) || qtd <= 0)
            {
                MessageBox.Show("Quantidade inválida.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!decimal.TryParse(txtItemAvulsoPreco.Text.Replace("R$", "").Replace(" ", "").Replace(",", "."), out decimal preco) || preco < 0)
            {
                MessageBox.Show("Preço inválido.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Carrinho.Add(new ItemCupom
            {
                IdProduto = 0,
                Codigo = "AVULSO",
                Descricao = txtItemAvulsoDesc.Text + (string.IsNullOrWhiteSpace(txtItemAvulsoComp.Text) ? "" : " - " + txtItemAvulsoComp.Text),
                Quantidade = (int)qtd,
                PrecoUnitario = preco,
                DescontoUnitario = 0,
                Total = qtd * preco
            });

            AtualizarTotais();
            FecharTelaItemAvulso();
        }

        // F5 - BUSCA PRODUTOS (já existe como AbrirTelaBusca/FecharTelaBusca)

        // F6 - BAIXA CLIENTES
        private void AbrirTelaBaixaClientes()
        {
            txtBaixaClienteID.Clear();
            listaBaixaClientes.ItemsSource = null;
            txtBaixaAtraso.Text = "R$ 0,00";
            txtBaixaAberto.Text = "R$ 0,00";
            OverlayBaixaClientes.Visibility = Visibility.Visible;
            OverlayBaixaClientes.Focus(); // Foco no overlay para KeyDown funcionar
            txtBaixaClienteID.Focus();
        }

        private void FecharTelaBaixaClientes()
        {
            OverlayBaixaClientes.Visibility = Visibility.Collapsed;
            txtBusca.Focus();
        }

        private void btnEfetuarLancamento_Click(object sender, RoutedEventArgs e)
        {
            // F3 - Efetuar Lançamento (registrar pagamento de um documento)
            if (listaBaixaClientes.SelectedItem is DocumentoBaixa doc)
            {
                var result = MessageBox.Show(
                    $"Confirmar baixa do documento {doc.Documento}?\nValor: {doc.Valor:C2}",
                    "Confirmar Lançamento",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (var conn = new Microsoft.Data.Sqlite.SqliteConnection(DatabaseConfig.ConnectionString))
                    {
                        conn.Open();
                        conn.Execute("UPDATE tb_vendas SET status = 'pago' WHERE id = @Id", new { Id = doc.Documento });
                    }
                    MessageBox.Show("Lançamento efetuado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    CarregarBaixaCliente(txtBaixaClienteID.Text.Trim());
                }
            }
            else
            {
                MessageBox.Show("Selecione um documento na lista.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnMostrarTodas_Click(object sender, RoutedEventArgs e)
        {
            // Mostrar todos os documentos (incluindo pagos)
            if (string.IsNullOrEmpty(txtBaixaClienteID.Text.Trim()))
            {
                MessageBox.Show("Informe o ID do cliente.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            using (var conn = new Microsoft.Data.Sqlite.SqliteConnection(DatabaseConfig.ConnectionString))
            {
                conn.Open();
                var docs = conn.Query<DocumentoBaixa>(@"
                    SELECT v.id as Documento, v.data_hora as Data, v.total as Valor, v.status as Situacao
                    FROM tb_vendas v
                    WHERE v.id_cliente = @Id
                    ORDER BY v.data_hora DESC", new { Id = txtBaixaClienteID.Text.Trim() }).ToList();

                decimal atraso = docs.Where(d => d.Situacao == "pendente" && Convert.ToDateTime(d.Data) < DateTime.Now.AddDays(-30)).Sum(d => d.Valor);
                decimal aberto = docs.Where(d => d.Situacao == "pendente").Sum(d => d.Valor);

                txtBaixaAtraso.Text = $"R$ {atraso:N2}";
                txtBaixaAberto.Text = $"R$ {aberto:N2}";
                listaBaixaClientes.ItemsSource = docs;
            }
        }

        private void btnExportarPDFBaixa_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Funcionalidade de exportar PDF em desenvolvimento.\nSerá implementada com geração de relatório.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnEnviarEmailBaixa_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Funcionalidade de envio de e-mail em desenvolvimento.\nIntegração com SMTP necessária.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void txtBaixaClienteID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CarregarBaixaCliente(txtBaixaClienteID.Text.Trim());
            }
            else if (e.Key == Key.Escape)
            {
                FecharTelaBaixaClientes();
            }
        }

        private void OverlayBaixaClientes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                FecharTelaBaixaClientes();
                e.Handled = true;
            }
            else if (e.Key == Key.F3)
            {
                btnEfetuarLancamento_Click(null, null);
                e.Handled = true;
            }
        }

        private void CarregarBaixaCliente(string idCliente)
        {
            if (string.IsNullOrEmpty(idCliente)) return;

            using (var conn = new Microsoft.Data.Sqlite.SqliteConnection(DatabaseConfig.ConnectionString))
            {
                conn.Open();
                // Simula crediário - busca vendas pendentes do cliente
                var docs = conn.Query<DocumentoBaixa>(@"
                    SELECT v.id as Documento, v.data_hora as Data, v.total as Valor, v.status as Situacao
                    FROM tb_vendas v
                    WHERE v.id_cliente = @Id AND v.status IN ('pendente', 'crediario')
                    ORDER BY v.data_hora", new { Id = idCliente }).ToList();

                decimal atraso = docs.Where(d => d.Situacao == "pendente" && Convert.ToDateTime(d.Data) < DateTime.Now.AddDays(-30)).Sum(d => d.Valor);
                decimal aberto = docs.Where(d => d.Situacao == "pendente").Sum(d => d.Valor);

                txtBaixaAtraso.Text = $"R$ {atraso:N2}";
                txtBaixaAberto.Text = $"R$ {aberto:N2}";
                listaBaixaClientes.ItemsSource = docs;
            }
        }

        // F7 - MEU CAIXA
        private void AbrirTelaMeuCaixa()
        {
            CarregarMeuCaixa();
            OverlayMeuCaixa.Visibility = Visibility.Visible;
            OverlayMeuCaixa.Focus(); // Foco no overlay para KeyDown funcionar
        }

        private void FecharTelaMeuCaixa()
        {
            OverlayMeuCaixa.Visibility = Visibility.Collapsed;
            txtBusca.Focus();
        }

        private void OverlayMeuCaixa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                FecharTelaMeuCaixa();
                e.Handled = true;
            }
            else if (e.Key == Key.F3)
            {
                btnSuprimento_Click(null, null);
                e.Handled = true;
            }
            else if (e.Key == Key.F4)
            {
                btnSangria_Click(null, null);
                e.Handled = true;
            }
            else if (e.Key == Key.F5)
            {
                btnFecharCaixa_Click(null, null);
                e.Handled = true;
            }
        }

        private void btnSuprimento_Click(object sender, RoutedEventArgs e)
        {
            // F3 - Suprimento (entrada de dinheiro no caixa)
            OverlaySuprimento.Visibility = Visibility.Visible;
            OverlaySuprimento.Focus();
            txtSuprimentoValor.Text = "0,00";
            txtSuprimentoValor.SelectAll();
            txtSuprimentoValor.Focus();
        }

        private void btnSangria_Click(object sender, RoutedEventArgs e)
        {
            // F4 - Sangria (retirada de dinheiro do caixa)
            OverlaySangria.Visibility = Visibility.Visible;
            OverlaySangria.Focus();
            txtSangriaValor.Text = "0,00";
            txtSangriaValor.SelectAll();
            txtSangriaValor.Focus();
        }

        private void btnFecharCaixa_Click(object sender, RoutedEventArgs e)
        {
            // F5 - Fechar Caixa (redireciona para a tela de fechamento)
            FecharTelaMeuCaixa();
            AbrirTelaFechamento();
        }

        private void btnImprimirCaixa_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Funcionalidade de impressão/PDF em desenvolvimento.\nGerará relatório de fechamento de caixa.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnEnviarEmailCaixa_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Funcionalidade de envio de e-mail em desenvolvimento.\nIntegração com SMTP necessária.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CarregarMeuCaixa()
        {
            using (var conn = new Microsoft.Data.Sqlite.SqliteConnection(DatabaseConfig.ConnectionString))
            {
                conn.Open();
                var hoje = DateTime.Now.Date;

                var vendasHoje = conn.Query<VendaResumoCaixa>(@"
                    SELECT p.metodo, COALESCE(SUM(p.valor), 0) as Total
                    FROM tb_vendas v
                    JOIN tb_pagamentos p ON v.id = p.id_venda
                    WHERE date(v.data_hora) = @Hoje AND v.status = 'pago'
                    GROUP BY p.metodo", new { Hoje = hoje }).ToList();

                decimal totalDinheiro = vendasHoje.Where(v => v.Metodo == "dinheiro").Sum(v => v.Total);
                decimal totalCredito = vendasHoje.Where(v => v.Metodo.Contains("credito") || v.Metodo.Contains("crédito")).Sum(v => v.Total);
                decimal totalDebito = vendasHoje.Where(v => v.Metodo.Contains("debito") || v.Metodo.Contains("débito")).Sum(v => v.Total);
                decimal totalPix = vendasHoje.Where(v => v.Metodo == "pix").Sum(v => v.Total);
                decimal totalCrediario = vendasHoje.Where(v => v.Metodo == "crediario").Sum(v => v.Total);

                txtCaixaDinheiro.Text = $"R$ {totalDinheiro:N2}";
                txtCaixaCredito.Text = $"R$ {totalCredito:N2}";
                txtCaixaDebito.Text = $"R$ {totalDebito:N2}";
                txtCaixaPix.Text = $"R$ {totalPix:N2}";
                txtCaixaCrediario.Text = $"R$ {totalCrediario:N2}";
                txtCaixaTotal.Text = $"R$ {totalDinheiro + totalCredito + totalDebito + totalPix + totalCrediario:N2}";

                // Saldo abertura
                var sessao = conn.QueryFirstOrDefault<dynamic>("SELECT valor_fundo_troco FROM tb_sessao_caixa WHERE id = @Id", new { Id = Session.CurrentSessaoCaixaId });
                decimal abertura = sessao?.valor_fundo_troco ?? 0;
                txtCaixaAbertura.Text = $"R$ {abertura:N2}";

                // Suprimentos/Sangrias (simulado)
                txtCaixaSuprimentos.Text = "R$ 0,00";
                txtCaixaSangrias.Text = "R$ 0,00";
                txtCaixaEsperado.Text = $"R$ {abertura + totalDinheiro + totalCredito + totalDebito + totalPix + totalCrediario:N2}";
            }
        }

        // F10 - MENU MULTIFUNÇÃO
        private void AbrirMenuMultifuncao()
        {
            OverlayMenuMultifuncao.Visibility = Visibility.Visible;
            OverlayMenuMultifuncao.Focus(); // Foco no overlay para KeyDown funcionar
        }

        private void FecharMenuMultifuncao()
        {
            OverlayMenuMultifuncao.Visibility = Visibility.Collapsed;
            txtBusca.Focus();
        }

        private void FecharMenuMultifuncao_Click(object sender, RoutedEventArgs e)
        {
            FecharMenuMultifuncao();
        }

        private void Menu_F2_Cliente_Click(object sender, RoutedEventArgs e)
        {
            FecharMenuMultifuncao();
            AbrirTelaCliente();
        }

        private void Menu_F3_Produto_Click(object sender, RoutedEventArgs e)
        {
            FecharMenuMultifuncao();
            AbrirTelaProduto();
        }

        private void Menu_F4_NFe_Click(object sender, RoutedEventArgs e)
        {
            FecharMenuMultifuncao();
            AbrirOverlayNFeDiversa();
        }

        private void Menu_F5_Caixa_Click(object sender, RoutedEventArgs e)
        {
            FecharMenuMultifuncao();
            AbrirTelaMeuCaixa();
        }

        private void Menu_F6_Treinamento_Click(object sender, RoutedEventArgs e)
        {
            FecharMenuMultifuncao();
            MessageBox.Show("Treinamento - Acesse o manual do usuário", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Menu_F7_Graficos_Click(object sender, RoutedEventArgs e)
        {
            FecharMenuMultifuncao();
            MessageBox.Show("Gráficos - Em desenvolvimento", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Menu_F8_Troca_Click(object sender, RoutedEventArgs e)
        {
            FecharMenuMultifuncao();
            MessageBox.Show("Troca/Devolução - Funcionalidade removida", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Menu_F9_Baixa_Click(object sender, RoutedEventArgs e)
        {
            FecharMenuMultifuncao();
            AbrirTelaBaixaClientes();
        }

        private void MenuMultifuncao_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) { FecharMenuMultifuncao(); e.Handled = true; }
            else if (e.Key == Key.F2) { FecharMenuMultifuncao(); AbrirTelaCliente(); e.Handled = true; }
            else if (e.Key == Key.F3) { FecharMenuMultifuncao(); AbrirTelaProduto(); e.Handled = true; }
            else if (e.Key == Key.F4) { FecharMenuMultifuncao(); AbrirOverlayNFeDiversa(); e.Handled = true; }
            else if (e.Key == Key.F5) { FecharMenuMultifuncao(); AbrirTelaMeuCaixa(); e.Handled = true; }
            else if (e.Key == Key.F6) { FecharMenuMultifuncao(); AbrirOverlayTreinamento(); e.Handled = true; }
            else if (e.Key == Key.F7) { FecharMenuMultifuncao(); AbrirOverlayGraficos(); e.Handled = true; }
            else if (e.Key == Key.F8) { FecharMenuMultifuncao(); AbrirOverlayTrocaDevolucao(); e.Handled = true; }
            else if (e.Key == Key.F9) { FecharMenuMultifuncao(); AbrirTelaBaixaClientes(); e.Handled = true; }
        }

        private void AbrirTelaCliente()
        {
            OverlayCliente.Visibility = Visibility.Visible;
            OverlayCliente.Focus();
            txtClienteBusca.Clear();
            txtClienteNovoNome.Clear();
            txtClienteResultado.Visibility = Visibility.Collapsed;
            txtClienteBusca.Focus();
        }

        private void FecharOverlayCliente()
        {
            OverlayCliente.Visibility = Visibility.Collapsed;
            txtBusca.Focus();
        }

        private void FecharOverlayCliente_Click(object sender, RoutedEventArgs e) => FecharOverlayCliente();

        private void OverlayCliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) { FecharOverlayCliente(); e.Handled = true; }
        }

        private void txtClienteBusca_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BuscarClienteOverlay(txtClienteBusca.Text.Trim());
                e.Handled = true;
            }
            else if (e.Key == Key.Escape) { FecharOverlayCliente(); e.Handled = true; }
        }

        private void txtClienteNovo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CadastrarClienteOverlay(txtClienteNovoNome.Text.Trim());
                e.Handled = true;
            }
            else if (e.Key == Key.Escape) { FecharOverlayCliente(); e.Handled = true; }
        }

        private void BuscarClienteOverlay(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return;
            using (var conn = new Microsoft.Data.Sqlite.SqliteConnection(DatabaseConfig.ConnectionString))
            {
                conn.Open();
                var cliente = conn.QueryFirstOrDefault<dynamic>(@"
                    SELECT id, nome, cpf_cnpj, telefone, email 
                    FROM tb_clientes 
                    WHERE id = @Id OR nome LIKE @Nome OR cpf_cnpj = @Cpf",
                    new { Id = input, Nome = $"%{input}%", Cpf = input });
                if (cliente != null)
                {
                    txtClienteResultado.Text = $"Cliente: {cliente.nome}\nCPF/CNPJ: {cliente.cpf_cnpj}\nTel: {cliente.telefone}\nEmail: {cliente.email}";
                    txtClienteResultado.Visibility = Visibility.Visible;
                }
                else
                {
                    txtClienteResultado.Text = "Cliente não encontrado.";
                    txtClienteResultado.Visibility = Visibility.Visible;
                }
            }
        }

        private void CadastrarClienteOverlay(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome)) return;
            using (var conn = new Microsoft.Data.Sqlite.SqliteConnection(DatabaseConfig.ConnectionString))
            {
                conn.Open();
                conn.Execute("INSERT INTO tb_clientes (nome) VALUES (@Nome)", new { Nome = nome });
                txtClienteResultado.Text = "Cliente cadastrado com sucesso!";
                txtClienteResultado.Visibility = Visibility.Visible;
                txtClienteNovoNome.Clear();
            }
        }

        private void AbrirTelaProduto()
        {
            OverlayProduto.Visibility = Visibility.Visible;
            OverlayProduto.Focus();
            txtProdutoBusca.Clear();
            txtProdutoResultado.Visibility = Visibility.Collapsed;
            txtProdutoBusca.Focus();
        }

        private void FecharOverlayProduto()
        {
            OverlayProduto.Visibility = Visibility.Collapsed;
            txtBusca.Focus();
        }

        private void FecharOverlayProduto_Click(object sender, RoutedEventArgs e) => FecharOverlayProduto();

        private void OverlayProduto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) { FecharOverlayProduto(); e.Handled = true; }
        }

        private void txtProdutoBusca_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BuscarProdutoOverlay(txtProdutoBusca.Text.Trim());
                e.Handled = true;
            }
            else if (e.Key == Key.Escape) { FecharOverlayProduto(); e.Handled = true; }
        }

        private void BuscarProdutoOverlay(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return;
            var produto = _repositorio.BuscarPorCodigoDeBarras(input);
            if (produto == null && int.TryParse(input, out int id))
            {
                produto = _repositorio.BuscarPorId(id);
            }
            if (produto != null)
            {
                var msg = $"Produto: {produto.nome}\nCódigo: {produto.codigo_barras}\nPreço: R$ {produto.preco_venda:N2}\nEstoque: {produto.quantidade}";
                if (produto.em_promocao && produto.valor_promocional.HasValue)
                {
                    msg += $"\nPROMOÇÃO: R$ {produto.valor_promocional.Value:N2}";
                }
                txtProdutoResultado.Text = msg;
                txtProdutoResultado.Visibility = Visibility.Visible;
            }
            else
            {
                txtProdutoResultado.Text = "Produto não encontrado.";
                txtProdutoResultado.Visibility = Visibility.Visible;
            }
        }

        // F11 - ENCERRAR
        private void AbrirTelaEncerrar()
        {
            OverlayEncerrar.Visibility = Visibility.Visible;
            OverlayEncerrar.Focus();
        }

        private void FecharTelaEncerrar()
        {
            OverlayEncerrar.Visibility = Visibility.Collapsed;
            txtBusca.Focus();
        }

        private void FecharTelaEncerrar_Click(object sender, RoutedEventArgs e)
        {
            FecharTelaEncerrar();
        }

        private async void btnConfirmarEncerrar_Click(object sender, RoutedEventArgs e)
        {
            await EncerrarSistema();
        }

        // F12 - PROMOÇÃO
        private void AbrirTelaPromocao()
        {
            CarregarPromocoes();
            OverlayPromocao.Visibility = Visibility.Visible;
            OverlayPromocao.Focus();
        }

        private void FecharTelaPromocao()
        {
            OverlayPromocao.Visibility = Visibility.Collapsed;
            txtBusca.Focus();
        }

        private void CarregarPromocoes()
        {
            using (var conn = new Microsoft.Data.Sqlite.SqliteConnection(DatabaseConfig.ConnectionString))
            {
                conn.Open();
                var promos = conn.Query<PromocaoInfo>(@"
                    SELECT * FROM tb_promocoes 
                    WHERE (data_final IS NULL OR date(data_final) >= date('now'))
                    ORDER BY data_inicio DESC").ToList();
                listaPromocoes.ItemsSource = promos;
            }
        }

        // ==========================================
        // OVERLAYS DOS SUBMENUS DO MENU MULTIFUNÇÃO (F10)
        // ==========================================

        private void AbrirOverlayNFeDiversa()
        {
            OverlayNFeDiversa.Visibility = Visibility.Visible;
            OverlayNFeDiversa.Focus();
        }

        private void FecharOverlayNFeDiversa()
        {
            OverlayNFeDiversa.Visibility = Visibility.Collapsed;
            txtBusca.Focus();
        }

        private void FecharOverlayNFeDiversa_Click(object sender, RoutedEventArgs e) => FecharOverlayNFeDiversa();

        private void OverlayNFeDiversa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) { FecharOverlayNFeDiversa(); e.Handled = true; }
        }

        private void AbrirOverlayTreinamento()
        {
            OverlayTreinamento.Visibility = Visibility.Visible;
            OverlayTreinamento.Focus();
        }

        private void FecharOverlayTreinamento()
        {
            OverlayTreinamento.Visibility = Visibility.Collapsed;
            txtBusca.Focus();
        }

        private void FecharOverlayTreinamento_Click(object sender, RoutedEventArgs e) => FecharOverlayTreinamento();

        private void OverlayTreinamento_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) { FecharOverlayTreinamento(); e.Handled = true; }
        }

        private void AbrirOverlayGraficos()
        {
            OverlayGraficos.Visibility = Visibility.Visible;
            OverlayGraficos.Focus();
        }

        private void FecharOverlayGraficos()
        {
            OverlayGraficos.Visibility = Visibility.Collapsed;
            txtBusca.Focus();
        }

        private void FecharOverlayGraficos_Click(object sender, RoutedEventArgs e) => FecharOverlayGraficos();

        private void OverlayGraficos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) { FecharOverlayGraficos(); e.Handled = true; }
        }

        private void AbrirOverlayTrocaDevolucao()
        {
            OverlayTrocaDevolucao.Visibility = Visibility.Visible;
            OverlayTrocaDevolucao.Focus();
        }

        private void FecharOverlayTrocaDevolucao()
        {
            OverlayTrocaDevolucao.Visibility = Visibility.Collapsed;
            txtBusca.Focus();
        }

        private void FecharOverlayTrocaDevolucao_Click(object sender, RoutedEventArgs e) => FecharOverlayTrocaDevolucao();

        private void OverlayTrocaDevolucao_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) { FecharOverlayTrocaDevolucao(); e.Handled = true; }
        }

        // ==========================================
        // OVERLAYS SUPRIMENTO/SANGRIA (F3/F4 dentro do Meu Caixa)
        // ==========================================

        private void FecharOverlaySuprimento()
        {
            OverlaySuprimento.Visibility = Visibility.Collapsed;
            OverlayMeuCaixa.Focus(); // Volta foco pro Meu Caixa
        }

        private void OverlaySuprimento_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) { FecharOverlaySuprimento(); e.Handled = true; }
            else if (e.Key == Key.Enter) { btnConfirmarSuprimento_Click(null, null); e.Handled = true; }
        }

        private void txtSuprimentoValor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { btnConfirmarSuprimento_Click(null, null); e.Handled = true; }
            else if (e.Key == Key.Escape) { FecharOverlaySuprimento(); e.Handled = true; }
        }

        private void btnConfirmarSuprimento_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(txtSuprimentoValor.Text.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal valor) && valor > 0)
            {
                using (var conn = new Microsoft.Data.Sqlite.SqliteConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    conn.Execute("INSERT INTO tb_movimentacao_caixa (id_sessao_caixa, tipo, valor, data_hora, descricao) VALUES (@Sessao, 'suprimento', @Valor, @Data, 'Suprimento manual')",
                        new { Sessao = Session.CurrentSessaoCaixaId, Valor = valor, Data = DateTime.Now });
                }
                FecharOverlaySuprimento();
                CarregarMeuCaixa();
            }
        }

        private void FecharOverlaySangria()
        {
            OverlaySangria.Visibility = Visibility.Collapsed;
            OverlayMeuCaixa.Focus(); // Volta foco pro Meu Caixa
        }

        private void OverlaySangria_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) { FecharOverlaySangria(); e.Handled = true; }
            else if (e.Key == Key.Enter) { btnConfirmarSangria_Click(null, null); e.Handled = true; }
        }

        private void txtSangriaValor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { btnConfirmarSangria_Click(null, null); e.Handled = true; }
            else if (e.Key == Key.Escape) { FecharOverlaySangria(); e.Handled = true; }
        }

        private void btnConfirmarSangria_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(txtSangriaValor.Text.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal valor) && valor > 0)
            {
                using (var conn = new Microsoft.Data.Sqlite.SqliteConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    conn.Execute("INSERT INTO tb_movimentacao_caixa (id_sessao_caixa, tipo, valor, data_hora, descricao) VALUES (@Sessao, 'sangria', @Valor, @Data, 'Sangria manual')",
                        new { Sessao = Session.CurrentSessaoCaixaId, Valor = valor, Data = DateTime.Now });
                }
                FecharOverlaySangria();
                CarregarMeuCaixa();
            }
        }

        // Classes auxiliares para as novas telas
        public class VendaResumo
        {
            public long id { get; set; }
            public string id_cupom { get; set; }
            public DateTime data_hora { get; set; }
            public decimal subtotal { get; set; }
            public decimal desconto { get; set; }
            public decimal total { get; set; }
            public string status { get; set; }
            public decimal TotalDinheiro { get; set; }
            public decimal TotalCredito { get; set; }
            public decimal TotalDebito { get; set; }
            public decimal TotalPix { get; set; }
            public decimal TotalCrediario { get; set; }
            public decimal TotalOutros { get; set; }
        }

        public class ItemVendaDetalhe
        {
            public long id { get; set; }
            public string nome { get; set; }
            public decimal quantidade { get; set; }
            public decimal preco_unitario { get; set; }
            public decimal subtotal { get; set; }
        }

        public class PagamentoDetalhe
        {
            public string metodo { get; set; }
            public decimal valor { get; set; }
        }

        public class VendaDetalheCompleto
        {
            public VendaResumo Venda { get; set; }
            public List<ItemVendaDetalhe> Itens { get; set; }
            public List<PagamentoDetalhe> Pagamentos { get; set; }
        }

        public class DocumentoBaixa
        {
            public long Documento { get; set; }
            public DateTime Data { get; set; }
            public decimal Valor { get; set; }
            public string Situacao { get; set; }
        }

        public class VendaResumoCaixa
        {
            public string Metodo { get; set; }
            public decimal Total { get; set; }
        }

        public class PromocaoInfo
        {
            public int id_promocao { get; set; }
            public string nome_campanha { get; set; }
            public string descricao { get; set; }
            public DateTime? data_inicio { get; set; }
            public DateTime? data_final { get; set; }
            public decimal? valor_minimo { get; set; }
            public string segmento_produtos { get; set; }
            public string maxima_desconto { get; set; }
        }

        // Helper para mover foco
        private void MoveFocusNext()
        {
            var request = new TraversalRequest(FocusNavigationDirection.Next);
            (Keyboard.FocusedElement as UIElement)?.MoveFocus(request);
        }

        private void MoveFocusPrev()
        {
            var request = new TraversalRequest(FocusNavigationDirection.Previous);
            (Keyboard.FocusedElement as UIElement)?.MoveFocus(request);
        }

        // ==========================================
        // GERADOR E IMPRESSOR DE CUPOM TÉRMICO
        // ==========================================
        private void ImprimirRecibo(long idVenda, string codigoUnicoCupom, System.Collections.Generic.List<ItemCupom> itens, decimal subtotal, decimal desconto, decimal total, string metodoPagamento, decimal valorRecebido, decimal troco)
        {
            try
            {
                var printDialog = new System.Windows.Controls.PrintDialog();
                var doc = new System.Windows.Documents.FlowDocument();

                // Reduzido para 275 e margens zeradas nas laterais para respeitar os 72mm da impressora térmica
                doc.PageWidth = 275;
                doc.PagePadding = new System.Windows.Thickness(0, 10, 0, 20);
                doc.FontFamily = new System.Windows.Media.FontFamily("Courier New");
                doc.Foreground = System.Windows.Media.Brushes.Black;

                // RESOLVE O PROBLEMA DE TEXTO FINO: Força TUDO a ser impresso em Negrito
                doc.FontWeight = FontWeights.Bold;

                System.Windows.Media.TextOptions.SetTextFormattingMode(doc, System.Windows.Media.TextFormattingMode.Display);
                System.Windows.Media.TextOptions.SetTextRenderingMode(doc, System.Windows.Media.TextRenderingMode.Aliased);

                // Reduzido para 36 colunas para caber na área de impressão sem quebrar a linha e estragar o alinhamento
                int colunas = 36;

                // 1. LOGO DA EMPRESA (Usando nota.png)
                try
                {
                    var uri = new Uri("pack://application:,,,/nota.png", UriKind.Absolute);
                    var bitmap = new System.Windows.Media.Imaging.BitmapImage(uri);
                    var img = new System.Windows.Controls.Image
                    {
                        Source = bitmap,
                        Width = 140, // Tamanho da logo
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Margin = new System.Windows.Thickness(0, 0, 0, 10)
                    };
                    System.Windows.Media.RenderOptions.SetBitmapScalingMode(img, System.Windows.Media.BitmapScalingMode.HighQuality);
                    doc.Blocks.Add(new System.Windows.Documents.BlockUIContainer(img));
                }
                catch { }

                // 2. CABEÇALHO (Centralizado)
                var pCabecalho = new System.Windows.Documents.Paragraph();
                pCabecalho.TextAlignment = TextAlignment.Center;
                pCabecalho.LineHeight = 16;
                pCabecalho.Margin = new Thickness(0, 0, 0, 10);

                pCabecalho.Inlines.Add(new System.Windows.Documents.Run("CNPJ: 28.173.572/0001-91\nCNPJ: 52.356.963/0001-26\nCNPJ: 68.712.675/0001-05\n") { FontSize = 11 });
                pCabecalho.Inlines.Add(new System.Windows.Documents.Run("Rua Buarque de Macedo, 3158,\nCentro, Garibaldi-RS\n") { FontSize = 11 });
                pCabecalho.Inlines.Add(new System.Windows.Documents.Run(new string('-', colunas) + "\n") { FontSize = 11 });
                pCabecalho.Inlines.Add(new System.Windows.Documents.Run("DOCUMENTO AUXILIAR DA NOTA FISCAL\nDE CONSUMIDOR ELETRONICA\n") { FontWeight = FontWeights.Black, FontSize = 11 });
                pCabecalho.Inlines.Add(new System.Windows.Documents.Run(new string('-', colunas)) { FontSize = 11 });
                doc.Blocks.Add(pCabecalho);

                // 3. CORPO DO CUPOM (Sem código do produto)
                var sb = new System.Text.StringBuilder();

                sb.AppendLine("DESCRIÇÃO                   TOTAL(R$)");

                foreach (var item in itens)
                {
                    sb.AppendLine();
                    // Descrição na primeira linha
                    string desc = item.Descricao.Length > colunas ? item.Descricao.Substring(0, colunas) : item.Descricao;
                    sb.AppendLine(desc);

                    // Quantidade, UN e Preço na esquerda, Total na direita
                    string qtd = item.Quantidade.ToString("0.###");
                    string un = "UN";
                    string vlUn = item.PrecoUnitario.ToString("N2");
                    string tot = item.Total.ToString("N2");

                    string linhaValores = $"{qtd} {un} X {vlUn}";
                    sb.AppendLine(AlinharLinha(linhaValores, tot, colunas));
                }
                sb.AppendLine(new string('-', colunas));

                // Totais (Agora soma a quantidade real de itens, e não a quantidade de linhas!)
                decimal qtdeTotalItens = itens.Sum(i => i.Quantidade);
                sb.AppendLine(AlinharLinha("Qtde. Total de Itens", qtdeTotalItens.ToString("0.###"), colunas));
                sb.AppendLine(AlinharLinha("Subtotal R$", subtotal.ToString("N2"), colunas));
                if (desconto > 0)
                    sb.AppendLine(AlinharLinha("Descontos R$", desconto.ToString("N2"), colunas));

                doc.Blocks.Add(new System.Windows.Documents.Paragraph(new System.Windows.Documents.Run(sb.ToString())) { FontSize = 11, Margin = new Thickness(0), LineHeight = 16 });

                // 4. VALOR A PAGAR EM DESTAQUE
                var pTotal = new System.Windows.Documents.Paragraph();
                pTotal.Margin = new Thickness(0, 2, 0, 10);
                pTotal.TextAlignment = TextAlignment.Left; // Alinha nativamente à direita, sem usar espaços falsos

                // Texto normal, número gigante e chamativo!
                pTotal.Inlines.Add(new System.Windows.Documents.Run("VALOR TOTAL  R$ ") { FontSize = 11 });
                pTotal.Inlines.Add(new System.Windows.Documents.Run(total.ToString("N2") + "\n") { FontWeight = FontWeights.Black, FontSize = 16 });
                doc.Blocks.Add(pTotal);

                // 5. FORMA DE PAGAMENTO E TROCO
                var sbPagamento = new System.Text.StringBuilder();
                sbPagamento.AppendLine(AlinharLinha("FORMA PAGAMENTO", "VALOR PAGO R$", colunas));
                sbPagamento.AppendLine(AlinharLinha(metodoPagamento.ToUpper(), valorRecebido.ToString("N2"), colunas));
                sbPagamento.AppendLine(AlinharLinha("Troco R$", troco.ToString("N2"), colunas));
                sbPagamento.AppendLine(new string('-', colunas));
                doc.Blocks.Add(new System.Windows.Documents.Paragraph(new System.Windows.Documents.Run(sbPagamento.ToString())) { FontSize = 11, Margin = new Thickness(0), LineHeight = 16 });

                // 6. CÓDIGO DE BARRAS DA VENDA E RODAPÉ
                try
                {
                    // Gera o código de barras com a string recebida do momento exato da venda
                    var imgBarcode = GerarCodigoBarrasITF(codigoUnicoCupom);
                    doc.Blocks.Add(new System.Windows.Documents.BlockUIContainer(imgBarcode));

                    var pRodape = new System.Windows.Documents.Paragraph();
                    pRodape.Margin = new Thickness(0, 0, 0, 10);
                    pRodape.TextAlignment = TextAlignment.Center;

                    pRodape.Inlines.Add(new System.Windows.Documents.Run(codigoUnicoCupom + "\n") { FontSize = 12, FontWeight = FontWeights.Black });
                    pRodape.Inlines.Add(new System.Windows.Documents.Run($"\nData: {DateTime.Now:dd/MM/yyyy HH:mm:ss}\n") { FontSize = 11 });
                    pRodape.Inlines.Add(new System.Windows.Documents.Run("OBRIGADO PELA PREFERENCIA!") { FontWeight = FontWeights.Black, FontSize = 11 });
                    doc.Blocks.Add(pRodape);
                }
                catch { }

                printDialog.PrintDocument(((System.Windows.Documents.IDocumentPaginatorSource)doc).DocumentPaginator, $"Recibo T-PACE {idVenda}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Não foi possível imprimir o cupom.\n\nDetalhe: {ex.Message}", "Erro de Impressão", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Função Matemática Auxiliar
        private string AlinharLinha(string esquerda, string direita, int totalColunas)
        {
            if (esquerda.Length + direita.Length >= totalColunas)
                esquerda = esquerda.Substring(0, totalColunas - direita.Length - 1);

            return esquerda + direita.PadLeft(totalColunas - esquerda.Length);
        }

        // ==========================================
        // GERADOR NATIVO DE CÓDIGO DE BARRAS (CODE 39)
        // ==========================================
        // NOVO GERADOR DE BARRAS: Interleaved 2 of 5 (Metade da largura, linhas nítidas e fortes)
        private System.Windows.Controls.Image GerarCodigoBarrasITF(string texto)
        {
            // O padrão ITF exige um número par de dígitos. Se for ímpar, adicionamos um zero.
            if (texto.Length % 2 != 0) texto = "0" + texto;

            string[] padroes = { "NNWWN", "WNNNW", "NWNNW", "WWNNN", "NNWNW", "WNWNN", "NWWNN", "NNNWW", "WNNWN", "NWNWN" };

            int barraMagra = 2; // Grosso o suficiente para a cabeça térmica não borrar
            int barraLarga = 5;
            int altura = 70;

            // Calcula a largura total da imagem com antecedência
            int larguraTotal = (barraMagra * 4) + (barraLarga + barraMagra * 2);
            foreach (char c in texto)
            {
                string p = padroes[c - '0'];
                foreach (char peso in p) larguraTotal += peso == 'W' ? barraLarga : barraMagra;
            }

            var visual = new System.Windows.Media.DrawingVisual();
            using (var context = visual.RenderOpen())
            {
                // Pinta o fundo de branco
                context.DrawRectangle(System.Windows.Media.Brushes.White, null, new Rect(0, 0, larguraTotal, altura));

                int x = 0;

                // Padrão de Início (Start: 4 linhas finas intercaladas)
                context.DrawRectangle(System.Windows.Media.Brushes.Black, null, new Rect(x, 0, barraMagra, altura)); x += barraMagra * 2;
                context.DrawRectangle(System.Windows.Media.Brushes.Black, null, new Rect(x, 0, barraMagra, altura)); x += barraMagra * 2;

                // A mágica: desenha os pares de números (um na barra preta, um no espaço em branco)
                for (int i = 0; i < texto.Length; i += 2)
                {
                    string p1 = padroes[texto[i] - '0'];     // Número 1 vira as barras pretas
                    string p2 = padroes[texto[i + 1] - '0']; // Número 2 vira os espaços brancos

                    for (int j = 0; j < 5; j++)
                    {
                        int wBar = p1[j] == 'W' ? barraLarga : barraMagra;
                        context.DrawRectangle(System.Windows.Media.Brushes.Black, null, new Rect(x, 0, wBar, altura));
                        x += wBar;

                        int wSpc = p2[j] == 'W' ? barraLarga : barraMagra;
                        x += wSpc;
                    }
                }

                // Padrão de Fim (Stop: Larga preta, Fina branca, Fina preta)
                context.DrawRectangle(System.Windows.Media.Brushes.Black, null, new Rect(x, 0, barraLarga, altura)); x += barraLarga + barraMagra;
                context.DrawRectangle(System.Windows.Media.Brushes.Black, null, new Rect(x, 0, barraMagra, altura)); x += barraMagra;
            }

            var rtb = new System.Windows.Media.Imaging.RenderTargetBitmap(larguraTotal, altura, 96, 96, System.Windows.Media.PixelFormats.Pbgra32);
            rtb.Render(visual);

            var img = new System.Windows.Controls.Image
            {
                Source = rtb,
                Width = larguraTotal,
                Height = altura,
                Margin = new Thickness(0, 15, 0, 5),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            System.Windows.Media.RenderOptions.SetBitmapScalingMode(img, System.Windows.Media.BitmapScalingMode.NearestNeighbor);
            return img;
        }

        // ==========================================
        // EVENTOS DE CLIQUE DO RODAPÉ
        // ==========================================
        private void Rodape_F1_Click(object sender, MouseButtonEventArgs e) => AbrirTelaAjuda();
        private void Rodape_F2_Click(object sender, MouseButtonEventArgs e) => AbrirTelaPagamento();
        private void Rodape_F3_Click(object sender, MouseButtonEventArgs e) => AbrirTelaVendasDia();
        private void Rodape_F4_Click(object sender, MouseButtonEventArgs e) => AbrirTelaItemAvulso();
        private void Rodape_F5_Click(object sender, MouseButtonEventArgs e) => AbrirTelaBusca();
        private void Rodape_F6_Click(object sender, MouseButtonEventArgs e) => AbrirTelaBaixaClientes();
        private void Rodape_F7_Click(object sender, MouseButtonEventArgs e) => AbrirTelaMeuCaixa();
        private void Rodape_F9_Click(object sender, MouseButtonEventArgs e) => MinimizarSistema();
        private void Rodape_F10_Click(object sender, MouseButtonEventArgs e) => AbrirMenuMultifuncao();
        private void Rodape_F11_Click(object sender, MouseButtonEventArgs e) => _ = EncerrarSistema();
        private void Rodape_F12_Click(object sender, MouseButtonEventArgs e) => AbrirTelaPromocao();

        public class ItemCupom : INotifyPropertyChanged
        {
            public int IdProduto { get; set; } // <--- ADICIONE ESTA LINHA AQUI

            private int _quantidade;
            private decimal _total;

            public string Codigo { get; set; } = string.Empty;
            public string Descricao { get; set; } = string.Empty;
            public decimal PrecoUnitario { get; set; }
            public decimal DescontoUnitario { get; set; }

            public int Quantidade
            {
                get => _quantidade;
                set
                {
                    _quantidade = value;
                    OnPropertyChanged(nameof(Quantidade));
                }
            }

            public decimal Total
            {
                get => _total;
                set
                {
                    _total = value;
                    OnPropertyChanged(nameof(Total));
                }
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}