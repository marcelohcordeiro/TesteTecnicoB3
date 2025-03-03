import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Titulo } from '../types/titulos.type';
import { Simulacao } from '../types/simulacao.type';

@Injectable()
export class TituloService {
  constructor(private httpClient: HttpClient) {}

  protected UrlServiceV1: string = 'https://localhost:5175/titulo/';

  getTitulos(): Observable<Titulo[]> {
    return this.httpClient.get<Titulo[]>(this.UrlServiceV1);
  }

  getTitulosById(idTitulo: string): Observable<Titulo[]> {
    return this.httpClient.get<Titulo[]>(this.UrlServiceV1 + idTitulo);
  }

  simularTitulo(
    idTitulo: string,
    valorInicial: string,
    valorAporteMensal: string,
    qtdeMeses: string
  ): Observable<Simulacao> {
    return this.httpClient.get<Simulacao>(
      this.UrlServiceV1 +
        'simulacao/' +
        idTitulo +
        '/' +
        valorInicial +
        '/' +
        valorAporteMensal +
        '/' +
        qtdeMeses
    );
  }
}

//57:08 - fernanda kipper projeto fullstack
