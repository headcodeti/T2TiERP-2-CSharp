﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SISGMFWeb.ServicoSISGMFReference;

namespace SISGMFWeb.Account
{
    public partial class Mercado : System.Web.UI.Page
    {
        private int IdMercado;
        private long RodapeTotalVendedores;
        private long RodapeTotalPresencas;
        private long RodapeTotalAusencias;
        private decimal RodapeTotalTaxas;
        private decimal RodapeTotalPagamentos;
        private decimal RodapeTotalDevido;

        protected void Page_Load(object sender, EventArgs e)
        {
            LabelMensagens.Text = "";
            AdicionarCabecalho();
            //
            RodapeTotalVendedores = 0;
            RodapeTotalPresencas = 0;
            RodapeTotalAusencias = 0;
            RodapeTotalTaxas = 0;
            RodapeTotalPagamentos = 0;
            RodapeTotalDevido = 0;
            //
            if (Session["MercadoNome"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                if (Session["UsuarioAdministrador"].ToString() != "S")
                {
                    if (Session["AcessoDisplayMercado"].ToString() != "S")
                    {
                        Response.Redirect("SemAcesso.aspx");
                    }
                }
                LabelTitulo.Text = "Display do Mercado: " + Session["MercadoNome"].ToString() +
                                    " - Maxima Autoridade Executiva: " + Session["MercadoResponsavel"].ToString();
                IdMercado = int.Parse(Session["MercadoId"].ToString());
            }
            //
            CarregarDados();
            TimerDisplay.Enabled = true;
        }

        private void AdicionarCabecalho()
        {
            TableHeaderRow LinhaCabecalho = new TableHeaderRow();

            LinhaCabecalho.BackColor = System.Drawing.Color.CadetBlue;
            LinhaCabecalho.ForeColor = System.Drawing.Color.White;

            TableHeaderCell ColunaCabecalhoArea = new TableHeaderCell();
            ColunaCabecalhoArea.Text = "Zona/Área";
            LinhaCabecalho.Cells.Add(ColunaCabecalhoArea);

            TableHeaderCell ColunaCabecalhoTotalVendedores = new TableHeaderCell();
            ColunaCabecalhoTotalVendedores.Text = "Total Vendedores";
            LinhaCabecalho.Cells.Add(ColunaCabecalhoTotalVendedores);

            TableHeaderCell ColunaCabecalhoPresencas = new TableHeaderCell();
            ColunaCabecalhoPresencas.Text = "Presenças";
            LinhaCabecalho.Cells.Add(ColunaCabecalhoPresencas);

            TableHeaderCell ColunaCabecalhoAusencias = new TableHeaderCell();
            ColunaCabecalhoAusencias.Text = "Ausencias";
            LinhaCabecalho.Cells.Add(ColunaCabecalhoAusencias);

            TableHeaderCell ColunaCabecalhoTaxas = new TableHeaderCell();
            ColunaCabecalhoTaxas.Text = "Total Taxas";
            LinhaCabecalho.Cells.Add(ColunaCabecalhoTaxas);

            TableHeaderCell ColunaCabecalhoValorPago = new TableHeaderCell();
            ColunaCabecalhoValorPago.Text = "Total Valor Pago";
            LinhaCabecalho.Cells.Add(ColunaCabecalhoValorPago);

            TableHeaderCell ColunaCabecalhoValorDevido = new TableHeaderCell();
            ColunaCabecalhoValorDevido.Text = "Total Valor Devido";
            LinhaCabecalho.Cells.Add(ColunaCabecalhoValorDevido);

            TableDados.Rows.Add(LinhaCabecalho);
        }

        private void AdicionarRodape()
        {
            TableFooterRow LinhaRodape = new TableFooterRow();

            LinhaRodape.Font.Bold = true;
            LinhaRodape.BackColor = System.Drawing.Color.CadetBlue;
            LinhaRodape.ForeColor = System.Drawing.Color.White;

            TableCell ColunaRodapeArea = new TableCell();
            ColunaRodapeArea.HorizontalAlign = HorizontalAlign.Center;
            ColunaRodapeArea.Text = "Totais";
            LinhaRodape.Cells.Add(ColunaRodapeArea);

            TableCell ColunaRodapeTotalVendedores = new TableCell();
            ColunaRodapeTotalVendedores.HorizontalAlign = HorizontalAlign.Right;
            ColunaRodapeTotalVendedores.Text = RodapeTotalVendedores.ToString();
            LinhaRodape.Cells.Add(ColunaRodapeTotalVendedores);

            TableCell ColunaRodapePresencas = new TableCell();
            ColunaRodapePresencas.HorizontalAlign = HorizontalAlign.Right;
            ColunaRodapePresencas.Text = RodapeTotalPresencas.ToString();
            LinhaRodape.Cells.Add(ColunaRodapePresencas);

            TableCell ColunaRodapeAusencias = new TableCell();
            ColunaRodapeAusencias.HorizontalAlign = HorizontalAlign.Right;
            ColunaRodapeAusencias.Text = RodapeTotalAusencias.ToString();
            LinhaRodape.Cells.Add(ColunaRodapeAusencias);

            TableCell ColunaRodapeTaxas = new TableCell();
            ColunaRodapeTaxas.HorizontalAlign = HorizontalAlign.Right;
            ColunaRodapeTaxas.Text = RodapeTotalTaxas.ToString("#,###,##0.00");
            LinhaRodape.Cells.Add(ColunaRodapeTaxas);

            TableCell ColunaRodapeValorPago = new TableCell();
            ColunaRodapeValorPago.HorizontalAlign = HorizontalAlign.Right;
            ColunaRodapeValorPago.Text = RodapeTotalPagamentos.ToString("#,###,##0.00");
            LinhaRodape.Cells.Add(ColunaRodapeValorPago);

            TableCell ColunaRodapeValorDevido = new TableCell();
            ColunaRodapeValorDevido.HorizontalAlign = HorizontalAlign.Right;
            ColunaRodapeValorDevido.Text = RodapeTotalDevido.ToString("#,###,##0.00");
            LinhaRodape.Cells.Add(ColunaRodapeValorDevido);

            TableDados.Rows.Add(LinhaRodape);

            if (RodapeTotalPresencas > 0)
            {
                if (!RodapeTotalPresencas.ToString().Equals(TotalPresencasHidden.Value))
                {
                    TotalPresencasHidden.Value = RodapeTotalPresencas.ToString();
                    System.Media.SystemSounds.Asterisk.Play();
                }
            }

            if (RodapeTotalPagamentos > 0)
            {
                if (!RodapeTotalPagamentos.ToString().Equals(TotalPagamentosHidden.Value))
                {
                    TotalPagamentosHidden.Value = RodapeTotalPagamentos.ToString();
                    System.Media.SystemSounds.Exclamation.Play();
                }
            }
        }

        protected void CarregarDados()
        {
            LabelMensagens.Text = "Apresentando os dados para o dia " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");

            try
            {
                TableDados.Rows.Clear();
                AdicionarCabecalho();

                using (ServicoSISGMFClient Servico = new ServicoSISGMFClient())
                {
                    // Total de Vendedores por Área
                    ViewTotalVendedorAreaDTO TotalVendedorArea = new ViewTotalVendedorAreaDTO();
                    TotalVendedorArea.IdMercado = IdMercado;
                    List<ViewTotalVendedorAreaDTO> ListaServ = Servico.SelectViewTotalVendedorArea(TotalVendedorArea);

                    // Para cada Área monta uma linha na Table
                    foreach (ViewTotalVendedorAreaDTO objAdd in ListaServ)
                    {
                        TableRow Linha = new TableRow();

                        TableCell ColunaArea = new TableCell();
                        ColunaArea.Text = objAdd.Nome;
                        Linha.Cells.Add(ColunaArea);

                        TableCell ColunaTotalVendedores = new TableCell();
                        ColunaTotalVendedores.HorizontalAlign = HorizontalAlign.Right;
                        ColunaTotalVendedores.Text = objAdd.TotalVendedor.ToString();
                        Linha.Cells.Add(ColunaTotalVendedores);
                        RodapeTotalVendedores = RodapeTotalVendedores + objAdd.TotalVendedor;

                        // Presenças e Ausencias
                        ViewTotalPresencaAreaDTO TotalPresenca = new ViewTotalPresencaAreaDTO();
                        TotalPresenca.IdArea = objAdd.Id;
                        TotalPresenca.DataPresenca = DateTime.Today;
                        TotalPresenca.IdMercado = IdMercado;
                        List<ViewTotalPresencaAreaDTO> ListaPresenca = Servico.SelectViewTotalPresencaArea(TotalPresenca);

                        if (ListaPresenca.Count > 0)
                        {
                            TotalPresenca = ListaPresenca[0];
                        }
                        else
                        {
                            TotalPresenca.TotalPresenca = 0;
                        }

                        TableCell ColunaTotalPresencas = new TableCell();
                        ColunaTotalPresencas.HorizontalAlign = HorizontalAlign.Right;
                        ColunaTotalPresencas.Text = TotalPresenca.TotalPresenca.ToString();
                        Linha.Cells.Add(ColunaTotalPresencas);
                        RodapeTotalPresencas = RodapeTotalPresencas + TotalPresenca.TotalPresenca;

                        TableCell ColunaTotalAusencias = new TableCell();
                        ColunaTotalAusencias.HorizontalAlign = HorizontalAlign.Right;
                        ColunaTotalAusencias.Text = (objAdd.TotalVendedor - TotalPresenca.TotalPresenca).ToString();
                        Linha.Cells.Add(ColunaTotalAusencias);
                        RodapeTotalAusencias = RodapeTotalAusencias + (objAdd.TotalVendedor - TotalPresenca.TotalPresenca);


                        // Pagamentos e Inadimplências
                        ViewTotalPagamentoAreaDTO TotalPagamento = new ViewTotalPagamentoAreaDTO();
                        TotalPagamento.IdArea = objAdd.Id;
                        TotalPagamento.DataPagamento = DateTime.Today;
                        TotalPagamento.IdMercado = IdMercado;
                        List<ViewTotalPagamentoAreaDTO> ListaPagamento = Servico.SelectViewTotalPagamentoArea(TotalPagamento);

                        if (ListaPagamento.Count > 0)
                        {
                            TotalPagamento = ListaPagamento[0];
                        }
                        else
                        {
                            TotalPagamento.TotalPagamento = 0;
                            TotalPagamento.TotalTaxa = 0;
                        }

                        TableCell ColunaTotalTaxas = new TableCell();
                        ColunaTotalTaxas.HorizontalAlign = HorizontalAlign.Right;
                        ColunaTotalTaxas.Text = TotalPagamento.TotalTaxa.Value.ToString("#,###,##0.00");
                        Linha.Cells.Add(ColunaTotalTaxas);
                        RodapeTotalTaxas = RodapeTotalTaxas + TotalPagamento.TotalTaxa.Value;

                        TableCell ColunaTotalPagamentos = new TableCell();
                        ColunaTotalPagamentos.HorizontalAlign = HorizontalAlign.Right;
                        ColunaTotalPagamentos.Text = TotalPagamento.TotalPagamento.Value.ToString("#,###,##0.00");
                        Linha.Cells.Add(ColunaTotalPagamentos);
                        RodapeTotalPagamentos = RodapeTotalPagamentos + TotalPagamento.TotalPagamento.Value;

                        TableCell ColunaTotalDevido = new TableCell();
                        ColunaTotalDevido.HorizontalAlign = HorizontalAlign.Right;
                        ColunaTotalDevido.Text = (TotalPagamento.TotalTaxa - TotalPagamento.TotalPagamento).Value.ToString("#,###,##0.00");
                        Linha.Cells.Add(ColunaTotalDevido);
                        RodapeTotalDevido = RodapeTotalDevido + (TotalPagamento.TotalTaxa.Value - TotalPagamento.TotalPagamento.Value);

                        TableDados.Rows.Add(Linha);
                    }

                }

                AdicionarRodape();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /*
        private void TimerDisplay_Tick(object sender, EventArgs e)
        {
            CarregarDados();
        }
        */
        protected void LinkButtonHistorico_Click1(object sender, EventArgs e)
        {
            Response.Redirect("MercadoConsulta.aspx?id_mercado=" + IdMercado.ToString());
        }

    }
}