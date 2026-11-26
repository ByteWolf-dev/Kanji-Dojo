import { Component, input } from '@angular/core';
import { CardDto } from '../dtos/card-dto';

@Component({
    selector: 'app-card',
    imports: [],
    templateUrl: './card.html',
    styleUrl: './card.css'
})
export class Card {

    card = input.required<CardDto>();

}