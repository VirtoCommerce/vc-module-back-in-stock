<template>
  <VcWidget
    icon="lucide-bell-ring"
    :title="$t('BACK_IN_STOCK.WIDGET')"
    :value="count ?? '…'"
    :disabled="count === undefined"
    @click="open"
  />
</template>

<script setup lang="ts">
// back-in-stock-subscription-widget on the contact blade (customerDetail1): the contact's subscriptions count
import { ref, watch } from "vue";
import { useApiClient, useBlade, useWidgetTrigger } from "@vc-shell/framework";
import { VcWidget } from "@vc-shell/framework/ui";
import { BackInStockSubscriptionClient } from "../../api_client/virtocommerce.backinstock";

// the contact blade's param is the contact (member) id
const { param, openBlade } = useBlade();
const { getApiClient } = useApiClient(BackInStockSubscriptionClient);
const count = ref<number>();

async function refresh() {
  if (!param.value) return;
  const r = await (await getApiClient()).search({ memberId: param.value, take: 0 }).catch(() => undefined);
  if (r) count.value = r.totalCount ?? 0;
}
watch(param, refresh, { immediate: true });
useWidgetTrigger({ onRefresh: refresh });

const open = () =>
  count.value !== undefined &&
  openBlade({ name: "BackInStockSubscriptions", param: param.value, onClose: refresh });
</script>
