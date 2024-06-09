import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminConstructoraComponent } from './admin-constructora.component';

describe('AdminConstructoraComponent', () => {
  let component: AdminConstructoraComponent;
  let fixture: ComponentFixture<AdminConstructoraComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminConstructoraComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdminConstructoraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
