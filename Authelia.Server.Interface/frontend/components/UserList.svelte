<div>
    <List
      class="demo-list"
      twoLine
      avatarList
      singleSelection
    >
      {#each $Store as item}
        <Item
          disabled={item.disabled}
        >
          <Graphic
            style="background-image: url(https://place-hold.it/40x40?text={item.userName
              .split(' ')
              .map((val) => val.substring(0, 1))
              .join('')}&fontsize=16);"
          />
          <Text>
            <PrimaryText>{item.userName}</PrimaryText>
            <SecondaryText>Test</SecondaryText>
          </Text>
        </Item>
      {/each}
    </List>
  </div>
   
  <script>
    import List, {
      Item,
      Graphic,
      Meta,
      Text,
      PrimaryText,
      SecondaryText,
    } from '@smui/list';
    import { onMount } from 'svelte';
    import Store from "../stores/users-store.js";
   
    onMount(()=> {
        Store.initialize(10 * 60 * 1000);
    })
  </script>
   
  <style>
    * :global(.demo-list) {
      max-width: 600px;
    }
  </style>