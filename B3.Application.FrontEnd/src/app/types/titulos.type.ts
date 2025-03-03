import { Indexador } from './indexador.type';
import { TipoTitulo } from './tipo-titulo.type';

export type Titulo = {
  idTitulo: string;
  nomeTitulo: string;
  idTipoTitulo: number;
  tipoTitulo: TipoTitulo;
  posFixado: boolean;
  idIndexador: number;
  indexador: Indexador;
  taxaRendimento: string;
};
