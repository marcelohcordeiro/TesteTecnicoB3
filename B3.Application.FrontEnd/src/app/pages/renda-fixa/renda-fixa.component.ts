import { Component, inject } from '@angular/core';
import { TituloService } from '../../services/titulo-service.service';
import { Titulo } from '../../types/titulos.type';
import { TitulosComponent } from '../../components/titulos/titulos.component';

@Component({
  selector: 'app-renda-fixa',
  imports: [TitulosComponent],
  providers: [TituloService],
  templateUrl: './renda-fixa.component.html',
  styleUrl: './renda-fixa.component.css',
})
export class RendaFixaComponent {
  private tituloService = inject(TituloService);
  meuBooleano = false;
  nome = 'Marcelo';

  public titulos!: Titulo[];

  atualizaBooleano(valor: boolean) {
    this.meuBooleano = valor;
  }

  buttonTesteSubmit() {
    console.log('teste do tchellinho');
  }

  ngOnInit() {
    this.tituloService.getTitulos().subscribe(
      (ret) => {
        this.titulos = ret;
        console.log(ret);
      },
      (error) => console.log(error)
    );
  }
}
