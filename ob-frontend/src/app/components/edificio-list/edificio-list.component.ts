import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { IEdificioModel } from '../../interfaces/iedificio-model';  // Adjust the import according to your project structure

@Component({
  selector: 'app-edificio-list-item',
  standalone: true,
  imports: [CommonModule, MatButtonModule],
  templateUrl: './edificio-list.component.html',
  styleUrls: ['./edificio-list.component.css']
})
export class EdificioListComponent {
  @Input() edificio!: IEdificioModel;
  @Output() delete = new EventEmitter<{ nombre: string, direccion: string }>();
  @Output() select = new EventEmitter<IEdificioModel>();



  onSelect() {
    this.select.emit(this.edificio);
  }
}