import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { RendaFixaComponent } from './pages/renda-fixa/renda-fixa.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'home', component: HomeComponent },
  { path: 'renda-fixa', component: RendaFixaComponent },
];
