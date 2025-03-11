import { Component, inject } from '@angular/core';
import { TituloService } from '../../services/titulo-service.service';
import { Titulo } from '../../types/titulos.type';
import { TitulosComponent } from '../../components/titulos/titulos.component';

@Component({
  selector: 'app-renda-fixa',
  imports: [TitulosComponent],
  providers: [TituloService],
  templateUrl: './renda-fixa.component.html',
})
export class RendaFixaComponent {
  private readonly tituloService = inject(TituloService);
  public titulos!: Titulo[];

  

  ngOnInit() {
    this.tituloService.getTitulos().subscribe({
      next: (ret: any) => {
        this.titulos = ret;
        console.log(ret);
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }
}
