import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeptoListItemComponent } from './depto-list-item.component';

describe('DeptoListItemComponent', () => {
  let component: DeptoListItemComponent;
  let fixture: ComponentFixture<DeptoListItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeptoListItemComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DeptoListItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
