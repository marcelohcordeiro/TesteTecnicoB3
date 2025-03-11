import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Titulo } from '../types/titulos.type';
import { Simulacao } from '../types/simulacao.type';
import { SimulacaoInput } from '../types/simulacao-input.type';

@Injectable()
export class TituloService {
  constructor(private readonly httpClient: HttpClient) {}

  protected UrlServiceV1: string = 'https://localhost:5175/titulo/';

  getTitulos(): Observable<Titulo[]> {
    return this.httpClient.get<Titulo[]>(this.UrlServiceV1 + 'renda-fixa');
  }

  getTitulosById(idTitulo: string): Observable<Titulo[]> {
    return this.httpClient.get<Titulo[]>(this.UrlServiceV1 + idTitulo);
  }

  simularTituloN(simulacaoInput: SimulacaoInput): Observable<Simulacao> {
    return this.httpClient.post<Simulacao>(
      this.UrlServiceV1 + 'simulacao',
      simulacaoInput
    );
  }
}
