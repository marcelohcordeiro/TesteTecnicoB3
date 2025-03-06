import { Component, inject } from '@angular/core';
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
import { CommonModule, CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-titulos',
  imports: [RouterModule, ReactiveFormsModule, CurrencyPipe, CommonModule],
  providers: [TituloService],
  templateUrl: './titulos.component.html',
})
export class TitulosComponent {
  private readonly tituloService = inject(TituloService);
  public simulacao: boolean = false;
  public titulos!: Titulo[];
  simulacaoForm!: FormGroup;
  numRegex = /^-?\d*[.,]?\d{0,2}$/;
  tituloCurrent!: Titulo;
  resultadoSimulacao!: Simulacao;
  mostrarResultadoSimulacao: boolean = false;
  /**
   *
   */
  constructor() {
    this.simulacaoForm = new FormGroup({
      valorInicial: new FormControl(null, [
        Validators.required,
        Validators.pattern(this.numRegex),
        Validators.min(1),
      ]),
      valorAporteMensal: new FormControl(null, [
        Validators.pattern(this.numRegex),
      ]),
      quantidadeMeses: new FormControl(null, [
        Validators.required,
        Validators.min(1),
      ]),
    });
  }

  ngOnInit() {
    this.getTitulos();
    this.simulacao = false;
  }

  getTitulos() {
    this.tituloService.getTitulos().subscribe(
      (ret) => {
        this.titulos = ret;
      },
      (error) => console.log(error)
    );
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

    this.tituloService
      .simularTitulo(
        this.tituloCurrent.idTitulo,
        this.simulacaoForm.value.valorInicial,
        this.simulacaoForm.value.valorAporteMensal,
        this.simulacaoForm.value.quantidadeMeses
      )
      .subscribe(
        (ret) => {
          this.resultadoSimulacao = ret;
          console.log('dentro do retorno', this.resultadoSimulacao);
          console.log('keys', Object.keys(this.resultadoSimulacao).length);
        },
        (error) => console.log(error)
      );
  }
}
