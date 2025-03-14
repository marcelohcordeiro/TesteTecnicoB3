import { Component, inject, LOCALE_ID } from '@angular/core';
import { TituloService } from '../../services/titulo-service.service';
import { Titulo } from '../../types/titulos.type';
import { RouterModule } from '@angular/router';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Simulacao } from '../../types/simulacao.type';
import {
  CommonModule,
  CurrencyPipe,
  registerLocaleData,
} from '@angular/common';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import localePt from '@angular/common/locales/pt';
import { SimulacaoInput } from '../../types/simulacao-input.type';

//REGISTRANDO A LOCALIDADE PT-BR
registerLocaleData(localePt);

@Component({
  selector: 'app-titulos',
  imports: [
    RouterModule,
    ReactiveFormsModule,
    CurrencyPipe,
    CommonModule,
    CurrencyMaskModule,
  ],
  providers: [TituloService, { provide: LOCALE_ID, useValue: 'pt-BR' }],
  templateUrl: './titulos.component.html',
})
export class TitulosComponent {
  private readonly tituloService = inject(TituloService);
  public simulacao: boolean = false;
  public titulos!: Titulo[];
  simulacaoForm!: FormGroup;
  moneyRegex = /^-?\d*[.,]?\d{0,2}$/;
  numberRegex = /^\d+(.\d{0,2})?$/;
  tituloCurrent!: Titulo;
  resultadoSimulacao!: Simulacao;
  mostrarResultadoSimulacao: boolean = false;
  simulacaoInput: SimulacaoInput = {
    idTitulo: '',
    valorInicial: '1',
    quantidadeMesesInvestimento: 1,
  };

  /**
   *
   */
  constructor() {
    this.simulacaoForm = new FormGroup({
      valorInicial: new FormControl(null, [
        Validators.required,
        Validators.pattern(this.moneyRegex),
        Validators.min(0.01),
      ]),
      valorAporteMensal: new FormControl(null, [
        Validators.pattern(this.moneyRegex),
        Validators.min(0.0),
      ]),
      quantidadeMeses: new FormControl(null, [
        Validators.required,
        Validators.min(2),
        Validators.pattern(this.numberRegex),
      ]),
    });
  }

  ngOnInit() {
    this.getTitulos();
    this.simulacao = false;
  }

  getTitulos() {
    this.tituloService.getTitulos().subscribe({
      next: (ret: any) => {
        this.titulos = ret;
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  ativarSimulacao(tituloSelecionado: Titulo) {
    this.inverterSimulacao();

    this.tituloCurrent = tituloSelecionado;
    console.log(this.tituloCurrent);
  }

  sairSimulacao() {
    this.inverterSimulacao();

    this.simulacaoForm.reset();
    this.mostrarResultadoSimulacao = false;
  }

  inverterSimulacao() {
    this.simulacao = !this.simulacao;
  }

  simularInvestimento() {
    this.mostrarResultadoSimulacao = true;

    //mapping
    this.simulacaoInput.idTitulo = this.tituloCurrent.idTitulo;
    this.simulacaoInput.valorInicial = this.simulacaoForm.value.valorInicial;
    this.simulacaoInput.quantidadeMesesInvestimento =
      this.simulacaoForm.value.quantidadeMeses;

    this.tituloService.simularTitulo(this.simulacaoInput).subscribe({
      next: (ret: any) => {
        this.resultadoSimulacao = ret;

        this.resultadoSimulacao.valorTotalBruto =
          this.arredondarPraBaixoDecimal(ret.valorTotalBruto);
        this.resultadoSimulacao.valorRendimento =
          this.arredondarPraBaixoDecimal(ret.valorRendimento);
        this.resultadoSimulacao.valorTotalInvestido =
          this.arredondarPraBaixoDecimal(ret.valorTotalInvestido);
        this.resultadoSimulacao.valorDescontoImpostoRenda =
          this.arredondarPraBaixoDecimal(ret.valorDescontoImpostoRenda);
        this.resultadoSimulacao.valorTotalLiquido =
          this.arredondarPraBaixoDecimal(ret.valorTotalLiquido);
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  arredondarPraBaixoDecimal(textoDecimal: string): string {
    return (
      parseInt((parseFloat(textoDecimal) * 100).toString()) / 100
    ).toString();
  }
}
