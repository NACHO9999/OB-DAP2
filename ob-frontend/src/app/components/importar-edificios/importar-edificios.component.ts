import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { AdminConstructoraService } from '../../services/admin-constructora.service';
import { IImporter } from '../../interfaces/iimporter';
import { CommonModule } from '@angular/common';
import { IImportRequestModel } from '../../interfaces/iimport-request-model';
import { LogoutButtonComponent } from '../logout/logout.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-importar-edificios',
  templateUrl: './importar-edificios.component.html',
  styleUrls: ['./importar-edificios.component.css'],
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    CommonModule,
    LogoutButtonComponent
  ],
  standalone: true,
})
export class ImportComponent implements OnInit {
  importForm: FormGroup;
  resultMessage: string = '';
  importers: IImporter[] = [];

  constructor(private fb: FormBuilder, private _service: AdminConstructoraService, private router: Router) {
    this.importForm = this.fb.group({
      nombre: ['', Validators.required],
      path: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadImporters();
  }

  onSubmit(): void {
    if (this.importForm.valid) {
      const formData = this.importForm.value;
      console.log(formData);
      this._service.importarEdificios(formData).subscribe({
        next: () => {
          this.resultMessage = 'Importación exitosa.';
        },
        error: (err: any) => {
          console.log(err);
          this.resultMessage = 'Error al importar.';
        }
      });
    }
  }
  loadImporters(): void {
    this._service.getImporters().subscribe({
      next: (data: IImporter[]) => {
        console.log(data);
        this.importers = data;
      },
      error: (err: any) => {
        console.error('Error in getImporters:', err);
      }
    });
  }
  volver(): void {
    this.router.navigate(['/admin_constructora']);
  }
}
