import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { IDeptoModel } from '../../interfaces/idepto-model';  // Adjust the import according to your project structure

@Component({
  selector: 'app-depto-list-item',
  standalone: true,
  imports: [CommonModule, MatButtonModule],
  templateUrl: './depto-list-item.component.html',
  styleUrls: ['./depto-list-item.component.css']
})
export class DeptoListItemComponent {
  @Input() depto!: IDeptoModel;
  @Output() select = new EventEmitter<IDeptoModel>();

  onSelect() {
    this.select.emit(this.depto);
  }
}
