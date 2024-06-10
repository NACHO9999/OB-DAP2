import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { ISolicitudModel } from '../../interfaces/isolicitud-model';

@Component({
  selector: 'app-solicitud-list-item',
  standalone: true,
  imports: [CommonModule, MatButtonModule],
  templateUrl: './solicitud-list-item.component.html',
  styleUrls: ['./solicitud-list-item.component.css']
})
export class SolicitudListItemComponent {
  @Input() solicitud!: ISolicitudModel;
  @Output() select = new EventEmitter<ISolicitudModel>();

  onSelect() {
    this.select.emit(this.solicitud);
  }
}
