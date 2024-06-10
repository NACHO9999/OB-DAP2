import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { SolicitudListItemComponent } from '../solicitud-list-item/solicitud-list-item.component';
import { ISolicitudModel } from '../../interfaces/isolicitud-model';
import { EncargadoService } from '../../services/encargado.service';
import { ICategoriaModel } from '../../interfaces/icategoria-model';
import { CategoriaService } from '../../services/categoria.service';
import { LogoutButtonComponent } from '../logout/logout.component';
import { Router } from '@angular/router';
import { catchError, of } from 'rxjs';

@Component({
  selector: 'app-visualizar-solicitudes',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatOptionModule,
    SolicitudListItemComponent,
    LogoutButtonComponent
  ],
  templateUrl: './visualizar-solicitudes.component.html',
  styleUrls: ['./visualizar-solicitudes.component.css']
})
export class VisualizarSolicitudesComponent implements OnInit {
  solicitudes: ISolicitudModel[] = [];
  categorias: ICategoriaModel[] = [];
  filteredSolicitudes: ISolicitudModel[] = [];
  filterForm: FormGroup;

  constructor(
    private encargadoService: EncargadoService,
    private categoriaService: CategoriaService,
    private fb: FormBuilder,
    private router: Router
  ) {
    this.filterForm = this.fb.group({
      categoria: ['']
    });
  }

  ngOnInit(): void {
    this.loadSolicitudes();
    this.loadCategorias();

    this.filterForm.get('categoria')?.valueChanges.subscribe(value => {
      this.filterSolicitudes(value);
    });
  }

  loadSolicitudes(): void {
    this.encargadoService.getAllSolicitudes().pipe(
      catchError((err) => {
        console.error('Error in getAllSolicitudes:', err);
        return of([]); // Return an empty array as a fallback
      })
    ).subscribe((solicitudes: ISolicitudModel[]) => {
      this.solicitudes = solicitudes;
      this.filteredSolicitudes = solicitudes;
    });
  }
  
  loadCategorias(): void {
    this.categoriaService.getCategorias().pipe(
      catchError((err) => {
        console.error('Error in getCategorias:', err);
        return of([]); // Return an empty array as a fallback
      })
    ).subscribe((categorias: ICategoriaModel[]) => {
      this.categorias = categorias;
    });
  }

  filterSolicitudes(categoria: string): void {
    if (categoria) {
      this.filteredSolicitudes = this.solicitudes.filter(solicitud => solicitud.categoria.nombre === categoria);
    } else {
      this.filteredSolicitudes = this.solicitudes;
    }
  }

  navigateBack(): void {
    this.router.navigate(['/encargado']);
  }
}
