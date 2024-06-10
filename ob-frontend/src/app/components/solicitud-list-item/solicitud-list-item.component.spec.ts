import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SolicitudListItemComponent } from './solicitud-list-item.component';

describe('SolicitudListItemComponent', () => {
  let component: SolicitudListItemComponent;
  let fixture: ComponentFixture<SolicitudListItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SolicitudListItemComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SolicitudListItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
