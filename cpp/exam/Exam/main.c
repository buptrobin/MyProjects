#include <stdio.h>
#include <stdlib.h>

typedef struct node
{
    struct node* next;
    int value;
} Node;

Node *NewNode(void);
void FreeLink(Node* pHead);
void MoveNode(Node* pHead, Node* pTargetTail);
void InitCards(Node* pCards, int count);
int CardsInOrder(Node* pHead);
int GetCardCount(int argc, char* argv[]);
Node* GetCardsTail(Node *pCards);

void main(int argc, char* argv[])
{
    int cardCount;
    if ((cardCount = GetCardCount(argc, argv)) < 0) {
        printf("Usage: %s cardcount\n", argv[0]);
        exit(0);
    }

    int round = ShuffleCards(cardCount);
    printf("Total Round=%d\n", round);
}

int ShuffleCards(int cardCount)
{
    if(cardCount <1 ) return 0;

    Node* pHand = NewNode();
    Node* pTable = NewNode();
    int rounds = 0;

    InitCards(pHand, cardCount);

    while(1)
    {
        ShuffleOneRound(pHand, pTable);
        rounds++;
        if(CardsInOrder(pHand)) break;
    }

    FreeLink(pHand);
    FreeLink(pTable);

    return rounds;
}

//initialize the cards on hand
void InitCards(Node* pCards, int count)
{
    if (!pCards || count<0) return;
    Node* pTail = pCards;
    int i;
    for(i=1;i<=count;i++)
    {
        Node* pNew = NewNode();
        pNew->value = i;
        pTail->next = pNew;
        pTail = pNew;
    }
}

//Shuffle one round cards
void ShuffleOneRound(Node* pHand, Node* pTable)
{
    if(!pHand || !pTable) exit(1);
    Node* pH;
    Node* pTableTail = pTable;
    Node* pHandTail = GetCardsTail(pHand);

    while(pHand->next!=NULL)
    {
        pH = pHand;
        while(pH->next != NULL)
        {
            MoveNode(pH, pTableTail); //move pH->next to pTable
            pTableTail = pTableTail->next;

            if(pH->next != NULL)
            {
                MoveNode(pH, pHandTail);
                pHandTail = pHandTail->next;
            }
        }
    }

    pHand->next = pTable->next;
    pTable->next = NULL;
}

Node* GetCardsTail(Node *pCards)
{
    if(!pCards) return pCards;
    while(pCards->next!=NULL)
    {
        pCards = pCards->next;
    }
    return pCards;
}

//check the the cards in hand are in order.
int CardsInOrder(Node* pHead)
{
    if(!pHead) return 1;
    Node* pCurr = pHead->next;
    Node* pNext;
    while(pCurr)
    {
        pNext = pCurr->next;
        if(!pNext) return 1;

        if(1 != (pNext->value - pCurr->value)) return 0;
        pCurr = pNext;
    }
    return 1;
}

void MoveNode(Node* pHead, Node* pTargetTail)
{
    if(!pHead->next) return;

    pTargetTail->next = pHead->next;
    pHead->next = (pHead->next)->next;
    pTargetTail = pTargetTail->next;
    pTargetTail->next = NULL;
}

Node *NewNode(void)
{
    Node* newnode = (Node*)malloc(sizeof(Node));
    if (!newnode) exit(1);
    newnode->next = NULL;
    return newnode;
}

int GetCardCount(int argc, char* argv[])
{
    if(argc < 2) return -1;
    return atoi(argv[1]);
}

void FreeLink(Node* pHead)
{
    Node *p1=pHead;
    Node *p2;
    while(p1)
    {
        p2 = p1->next;
        free(p1);
        p1 = p2;
    }
}
