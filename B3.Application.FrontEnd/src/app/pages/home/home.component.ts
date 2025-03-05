import { Component } from '@angular/core';
import { TituloService } from '../../services/titulo-service.service';
import { RouterModule } from '@angular/router';
@Component({
  selector: 'app-home',
  imports: [RouterModule],
  providers: [TituloService],
  templateUrl: './home.component.html',
})
export class HomeComponent {}
