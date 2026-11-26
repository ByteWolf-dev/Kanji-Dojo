import { Pronunciation } from '../enums/pronunciation';

export interface ItemReadingDto {
    id: number;
    itemId: number;
    readingKana: string;
    readingRomaji: string;
    pronunciation: Pronunciation;
}
