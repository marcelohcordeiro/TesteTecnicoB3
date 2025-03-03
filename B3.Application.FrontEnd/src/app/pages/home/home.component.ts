import { Component, inject } from '@angular/core';
import { TituloService } from '../../services/titulo-service.service';
import { RouterModule } from '@angular/router';
@Component({
  selector: 'app-home',
  imports: [RouterModule],
  providers: [TituloService],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent {}
