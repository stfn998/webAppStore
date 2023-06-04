import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderCurrentComponent } from './order-current.component';

describe('OrderCurrentComponent', () => {
  let component: OrderCurrentComponent;
  let fixture: ComponentFixture<OrderCurrentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OrderCurrentComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrderCurrentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
